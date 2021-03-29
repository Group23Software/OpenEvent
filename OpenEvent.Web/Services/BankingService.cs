using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenEvent.Data.Models.BankAccount;
using OpenEvent.Data.Models.User;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Exceptions;
using Stripe;
using BankAccount = OpenEvent.Data.Models.BankAccount.BankAccount;

namespace OpenEvent.Web.Services
{
    /// <inheritdoc />
    public class BankingService : IBankingService
    {
        private readonly ILogger<BankingService> Logger;
        private readonly ApplicationContext ApplicationContext;
        private readonly IMapper Mapper;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="applicationContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="appSettings"></param>
        public BankingService(ApplicationContext applicationContext, ILogger<BankingService> logger, IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            Logger = logger;
            ApplicationContext = applicationContext;
            Mapper = mapper;
            StripeConfiguration.ApiKey = appSettings.Value.StripeApiKey;
        }

        /// <inheritdoc />
        /// <exception cref="UserNotFoundException">Thrown when the user is not found</exception>
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
                var account = CreateAccount(user);
                user.StripeAccountId = account.Id;
                await ApplicationContext.SaveChangesAsync();
            }

            var options = new ExternalAccountCreateOptions()
            {
                ExternalAccount = addBankAccountBody.BankToken
            };
            
            var service = new ExternalAccountService();

            try
            {
                // request create bank account from the Stripe api
                var bank = (Stripe.BankAccount) service.Create(user.StripeAccountId, options);

                // create bank account using Stripe response
                var bankAccount = new BankAccount
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
            catch
            {
                Logger.LogWarning("Failed to save bank account");
                throw;
            }
        }

        /// <inheritdoc />
        /// <exception cref="UserNotFoundException">Thrown when the user is not found</exception>
        /// <exception cref="BankAccountNotFoundException">thrown when the bank account is not found</exception>
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
                throw new BankAccountNotFoundException();
            }
            
            var service = new AccountService();
            
            // request delete bank account from the Stripe api
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

        // Creates a Stripe account using user information
        private Account CreateAccount(User user)
        {
            // account create options
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
                // requests account create from the Stripe api 
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