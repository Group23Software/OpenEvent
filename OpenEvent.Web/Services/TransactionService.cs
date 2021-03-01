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
using OpenEvent.Web.Models.Intent;
using OpenEvent.Web.Models.Transaction;
using Serilog;
using Stripe;

namespace OpenEvent.Web.Services
{
    public interface ITransactionService
    {
        Task<TransactionViewModel> CreateIntent(CreateIntentBody createIntentBody);
        Task<TransactionViewModel> InjectPaymentMethod(InjectPaymentMethodBody injectPaymentMethodBody);
        Task CaptureIntentHook(); // Called by payment intent webhook
    }

    public class TransactionService : ITransactionService
    {
        private readonly ILogger<TransactionService> Logger;
        private readonly ApplicationContext ApplicationContext;
        private readonly IMapper Mapper;

        public TransactionService(ApplicationContext applicationContext, ILogger<TransactionService> logger,
            IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            Logger = logger;
            ApplicationContext = applicationContext;
            Mapper = mapper;
            StripeConfiguration.ApiKey = appSettings.Value.StripeApiKey;
        }

        public async Task<TransactionViewModel> CreateIntent(CreateIntentBody createIntentBody)
        {
            var user = await ApplicationContext.Users
                .Include(x => x.PaymentMethods)
                .FirstOrDefaultAsync(x => x.Id == createIntentBody.UserId);
            var e = await ApplicationContext.Events.AsSplitQuery()
                .Include(x => x.Host)
                .Include(x => x.Tickets)
                .FirstOrDefaultAsync(x => x.Id == createIntentBody.EventId);

            if (user == null)
            {
                Logger.LogInformation("User was not found");
                throw new UserNotFoundException();
            }

            if (user.StripeCustomerId == null || !user.PaymentMethods.Any())
                throw new Exception("Not enough stripe info");

            if (e == null)
            {
                Logger.LogInformation("Event was not found");
                throw new EventNotFoundException();
            }

            if (e.Host.StripeAccountId == null) throw new Exception("The host doesn't have enough stripe info");

            var options = new PaymentIntentCreateOptions()
            {
                Amount = createIntentBody.Amount,
                Currency = "gbp",
                OnBehalfOf = e.Host.StripeAccountId,
                Customer = user.StripeCustomerId,
                ConfirmationMethod = "automatic",
                ApplicationFeeAmount = 10,
                TransferData = new PaymentIntentTransferDataOptions()
                {
                    Destination = e.Host.StripeAccountId
                },
                PaymentMethodTypes = new List<string>
                {
                    "card"
                }
            };

            var service = new PaymentIntentService();

            try
            {
                var intent = service.Create(options);

                var ticket = e.Tickets.First(x => x.Transaction == null && x.User == null);

                Transaction transaction = new Transaction()
                {
                    Amount = intent.Amount,
                    User = user,
                    Paid = intent.Status == "succeeded",
                    StripeIntentId = intent.Id,
                    Start = DateTime.Now,
                    Updated = DateTime.Now,
                    Status = Enum.Parse<PaymentStatus>(intent.Status, true),
                    Ticket = ticket,
                    TicketId = ticket.Id,
                };

                await ApplicationContext.Transactions.AddAsync(transaction);
                await ApplicationContext.SaveChangesAsync();

                return Mapper.Map<TransactionViewModel>(transaction);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        public async Task<TransactionViewModel> InjectPaymentMethod(InjectPaymentMethodBody injectPaymentMethodBody)
        {
            var user = await ApplicationContext.Users
                .Include(x => x.PaymentMethods)
                .FirstOrDefaultAsync(x => x.Id == injectPaymentMethodBody.UserId);
            var transaction =
                await ApplicationContext.Transactions.FirstOrDefaultAsync(x =>
                    x.StripeIntentId == injectPaymentMethodBody.IntentId);

            if (user == null)
            {
                Logger.LogInformation("User was not found");
                throw new UserNotFoundException();
            }

            if (user.PaymentMethods.All(x => x.StripeCardId != injectPaymentMethodBody.CardId))
                throw new Exception("User doesn't own card");

            if (transaction == null) throw new Exception("Transaction not found");

            var service = new PaymentIntentService();

            try
            {
                var intent = service.Confirm(injectPaymentMethodBody.IntentId,
                    new PaymentIntentConfirmOptions()
                    {
                        PaymentMethod = injectPaymentMethodBody.CardId
                    });

                transaction.Updated = DateTime.Now;
                transaction.Status = Enum.Parse<PaymentStatus>(intent.Status, true);
                if (transaction.Status == PaymentStatus.succeeded)
                {
                    transaction.Paid = true;
                    transaction.End = DateTime.Now;
                }

                await ApplicationContext.SaveChangesAsync();

                return Mapper.Map<TransactionViewModel>(transaction);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Task CaptureIntentHook()
        {
            throw new System.NotImplementedException();
        }
    }
}