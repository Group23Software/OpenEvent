using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Exceptions;
using OpenEvent.Web.Models.Intent;
using OpenEvent.Web.Models.Ticket;
using OpenEvent.Web.Models.Transaction;
using Serilog;
using Stripe;
using Event = OpenEvent.Web.Models.Event.Event;

namespace OpenEvent.Web.Services
{
    public interface ITransactionService
    {
        Task<TransactionViewModel> CreateIntent(CreateIntentBody createIntentBody);
        Task<TransactionViewModel> InjectPaymentMethod(InjectPaymentMethodBody injectPaymentMethodBody);
        Task<TransactionViewModel> ConfirmIntent(ConfirmIntentBody confirmIntentBody);
        Task CaptureIntentHook(Stripe.Event @event); // Called by payment intent webhook
        Task CancelIntent(CancelIntentBody cancelIntentBody);
    }

    public class TransactionService : ITransactionService
    {
        private readonly ILogger<TransactionService> Logger;
        private readonly ApplicationContext ApplicationContext;
        private readonly IMapper Mapper;
        private readonly IServiceScopeFactory ScopeFactory;
        private readonly IRecommendationService RecommendationService;
        private readonly IEmailService EmailService;

        public TransactionService(
            ApplicationContext applicationContext,
            ILogger<TransactionService> logger,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            IServiceScopeFactory serviceScopeFactory,
            IRecommendationService recommendationService,
            IEmailService emailService)
        {
            Logger = logger;
            ApplicationContext = applicationContext;
            Mapper = mapper;
            StripeConfiguration.ApiKey = appSettings.Value.StripeApiKey;
            ScopeFactory = serviceScopeFactory;
            RecommendationService = recommendationService;
            EmailService = emailService;
        }

        public async Task<TransactionViewModel> CreateIntent(CreateIntentBody createIntentBody)
        {
            var user = await ApplicationContext.Users
                .Include(x => x.PaymentMethods)
                .FirstOrDefaultAsync(x => x.Id == createIntentBody.UserId);

            var e = await ApplicationContext.Events.AsSplitQuery()
                .Include(x => x.Host)
                .Include(x => x.Tickets)
                .Include(x => x.Transactions)
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
                ConfirmationMethod = "manual",
                ApplicationFeeAmount = 10,
                TransferData = new PaymentIntentTransferDataOptions()
                {
                    Destination = e.Host.StripeAccountId
                },
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                Metadata = new Dictionary<string, string>()
                {
                    {"EventId", e.Id.ToString()}
                },
            };

            var service = new PaymentIntentService();

            try
            {
                var intent = service.Create(options);

                var ticket = e.Tickets.FirstOrDefault(x => x.Available);

                if (ticket == null) throw new Exception("Out of tickets");

                e.TicketsLeft = e.Tickets.Count(x => x.Available) - 1;

                ticket.User = user;
                ticket.Available = false;

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
                    Event = e,
                    ClientSecret = intent.ClientSecret
                };

                await ApplicationContext.Transactions.AddAsync(transaction);
                await ApplicationContext.SaveChangesAsync();

                CheckOnIntent(transaction.StripeIntentId, ticket.Id, e.Id);

                return Mapper.Map<TransactionViewModel>(transaction);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        private void CheckOnIntent(string id, Guid ticketId, Guid eventId)
        {
            Task.Run(async () =>
            {
                Logger.LogInformation("Checking intent");
                // waits x amount of time for intent timeout
                await Task.Delay(TimeSpan.FromMinutes(20));
                Logger.LogInformation("The intent has timed out! Destroying!");

                using var scope = ScopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

                await Cancel(context, id, eventId, ticketId);

                Logger.LogInformation("Checked on intent");
                context = default;
            });
        }

        private async Task Cancel(ApplicationContext context, string id, Guid eventId, Guid ticketId)
        {
            var transaction = await context.Transactions.FirstOrDefaultAsync(x => x.StripeIntentId == id);

            var ticket = await context.Tickets.FirstOrDefaultAsync(x => x.Id == ticketId);

            var @event = await context.Events.FirstOrDefaultAsync(x => x.Id == eventId);

            if (@event == null) return;

            if (transaction == null) return;

            if (ticket == null) return;

            transaction.End = DateTime.Now;
            transaction.Status = PaymentStatus.canceled;
            transaction.Ticket = null;
            transaction.Updated = DateTime.Now;

            ticket.User = null;
            ticket.Available = true;
            ticket.Transaction = null;

            @event.TicketsLeft++;

            var service = new PaymentIntentService();

            try
            {
                Logger.LogInformation("Canceling the intent");
                service.Cancel(transaction.StripeIntentId);

                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Logger.LogWarning(e.Message);
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
                var intent = service.Update(injectPaymentMethodBody.IntentId,
                    new PaymentIntentUpdateOptions()
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

        public async Task<TransactionViewModel> ConfirmIntent(ConfirmIntentBody confirmIntentBody)
        {
            var transaction =
                await ApplicationContext.Transactions.FirstOrDefaultAsync(x =>
                    x.StripeIntentId == confirmIntentBody.IntentId);

            if (transaction == null) throw new Exception("Transaction not found");

            var service = new PaymentIntentService();

            try

            {
                var intent = service.Confirm(confirmIntentBody.IntentId);

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

        public async Task CaptureIntentHook(Stripe.Event stripeEvent)
        {
            try
            {
                if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    Logger.LogInformation("Payment intent succeeded");
                    
                    var stripeCharge = stripeEvent.Data.Object as PaymentIntent;

                    var transaction =
                        await ApplicationContext.Transactions.Include(x => x.User).AsSplitQuery().FirstOrDefaultAsync(x => x.StripeIntentId == stripeCharge.Id);

                    if (transaction != null)
                    {
                        transaction.Updated = DateTime.Now;
                        transaction.Status = PaymentStatus.succeeded;
                        transaction.Paid = true;
                        transaction.End = DateTime.Now;

                        await ApplicationContext.SaveChangesAsync();

                        await EmailService.SendAsync(transaction.User.Email, "OpenEvent",
                            "<h1>Ticket Purchased</h1><p>You have successfully purchased a ticket</p>",
                            "Ticket Purchased");

                        Logger.LogInformation("A successful payment was processed");
                    }
                    else
                    {
                        Logger.LogInformation("Transaction not found");
                    }
                }
                else if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                {
                    Logger.LogInformation("Payment intent failed");
                    
                    var intent = stripeEvent.Data.Object as PaymentIntent;

                    var transaction = await ApplicationContext.Transactions.FirstOrDefaultAsync(x =>
                        x.StripeIntentId == intent.Id);

                    if (transaction != null)
                    {
                        transaction.Updated = DateTime.Now;
                        transaction.End = DateTime.Now;
                        transaction.Paid = false;
                        transaction.Status = PaymentStatus.canceled;
                        transaction.Ticket = null;

                        await ApplicationContext.SaveChangesAsync();

                        await EmailService.SendAsync(transaction.User.Email, "OpenEvent",
                            "<h1>Payment Failure</h1><p>You tried to purchase a ticket but the payment failed</p>",
                            "Payment Failure");

                        Logger.LogInformation("A payment was not successful");
                    }
                    else
                    {
                        Logger.LogInformation("Transaction not found");
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogInformation(e.Message);
            }
        }

        public async Task CancelIntent(CancelIntentBody cancelIntentBody)
        {
            try
            {
                await Cancel(ApplicationContext, cancelIntentBody.Id, cancelIntentBody.EventId,
                    cancelIntentBody.TicketId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}