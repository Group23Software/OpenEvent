using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Exceptions;
using OpenEvent.Web.Models.BankAccount;
using OpenEvent.Web.Models.User;
using Stripe;
using BankAccount = OpenEvent.Web.Models.BankAccount.BankAccount;

namespace OpenEvent.Web.Services
{
    public interface IBankingService
    {
        Task<BankAccountViewModel> AddBankAccount(AddBankAccountBody addBankAccountBody);
        Task RemoveBankAccount(RemoveBankAccountBody removeBankAccountBody);
    }

    public class BankingService : IBankingService
    {
        private readonly ILogger<BankingService> Logger;
        private readonly ApplicationContext ApplicationContext;

        private readonly IMapper Mapper;

        public BankingService(ApplicationContext applicationContext, ILogger<BankingService> logger, IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            Logger = logger;
            ApplicationContext = applicationContext;
            Mapper = mapper;
            StripeConfiguration.ApiKey = appSettings.Value.StripeApiKey;
        }

        public async Task<BankAccountViewModel> AddBankAccount(AddBankAccountBody addBankAccountBody)
        {
            var user = await ApplicationContext.Users.Include(x => x.BankAccounts)
                .FirstOrDefaultAsync(x => x.Id == addBankAccountBody.UserId);

            if (user == null)
            {
                throw new UserNotFoundException();
            }

            if (user.StripeAccountId == null)
            {
                var account = await CreateAccount(user);
                user.StripeAccountId = account.Id;
                await ApplicationContext.SaveChangesAsync();
            }

            // var bankAccountCreateOptions = new BankAccountCreateOptions()
            // {
            //     Source = addBankAccountBody.BankToken
            // };

            var options = new ExternalAccountCreateOptions()
            {
                ExternalAccount = addBankAccountBody.BankToken
            };

            // var service = new BankAccountService();
            var service = new ExternalAccountService();

            try
            {
                var bank = (Stripe.BankAccount) service.Create(user.StripeAccountId, options);
                
                
                var bankAccount = new BankAccount()
                {
                    StripeBankAccountId = bank.Id,
                    Bank = bank.BankName,
                    Country = bank.Country,
                    Currency = bank.Currency,
                    Name = bank.AccountHolderName,
                    LastFour = bank.Last4
                };

                user.BankAccounts = new List<BankAccount> {bankAccount};

                await ApplicationContext.SaveChangesAsync();
                return Mapper.Map<BankAccountViewModel>(user.BankAccounts[0]);
            }
            catch (Exception e)
            {
                Logger.LogWarning(e.Message);
                throw;
            }
        }

        public async Task RemoveBankAccount(RemoveBankAccountBody removeBankAccountBody)
        {
            var user = await ApplicationContext.Users.Include(x => x.BankAccounts)
                .FirstOrDefaultAsync(x => x.Id == removeBankAccountBody.UserId);

            if (user == null)
            {
                throw new UserNotFoundException();
            }

            var bankAccount = user.BankAccounts.First();

            if (bankAccount == null)
            {
                // TODO Make custom
                throw new Exception();
            }
            
            var service = new AccountService();
            service.Delete(user.StripeAccountId);
            
            try
            {
                user.StripeAccountId = null;
                ApplicationContext.BankAccounts.Remove(bankAccount);
                await ApplicationContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Logger.LogWarning(e.Message);
                throw;
            }
        }

        private async Task<Account> CreateAccount(User user)
        {
            var options = new AccountCreateOptions()
            {
                BusinessType = "individual",
                TosAcceptance = new AccountTosAcceptanceOptions() {Date = DateTime.Now, Ip = "127.0.0.1"},
                BusinessProfile = new AccountBusinessProfileOptions()
                    {Mcc = "7991", Url = "http://www.harrisonbarker.co.uk"},
                Individual = new AccountIndividualOptions()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Dob = new DobOptions()
                    {
                        Year = user.DateOfBirth.Year,
                        Month = user.DateOfBirth.Month,
                        Day = user.DateOfBirth.Day
                    },
                    Address = new AddressOptions()
                    {
                        Line1 = user.Address.AddressLine1,
                        Line2 = user.Address.AddressLine2,
                        City = user.Address.City,
                        Country = user.Address.CountryCode,
                        PostalCode = user.Address.PostalCode
                    },
                    Email = user.Email,
                    Phone = user.PhoneNumber
                },
                Capabilities = new AccountCapabilitiesOptions
                {
                    CardPayments = new AccountCapabilitiesCardPaymentsOptions {Requested = true},
                    Transfers = new AccountCapabilitiesTransfersOptions {Requested = true},
                },
                Type = "custom"
            };

            var service = new AccountService();

            try
            {
                var account = service.Create(options);
                return account;
            }
            catch (Exception e)
            {
                Logger.LogWarning(e.Message);
                throw;
            }
        }
    }
}