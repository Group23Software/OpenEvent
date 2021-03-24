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
    /// <inheritdoc />
    public class TransactionService : ITransactionService
    {
        private readonly ILogger<TransactionService> Logger;
        private readonly ApplicationContext ApplicationContext;
        private readonly IMapper Mapper;
        private readonly IServiceScopeFactory ScopeFactory;
        private readonly IEmailService EmailService;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="applicationContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="appSettings"></param>
        /// <param name="serviceScopeFactory"></param>
        /// <param name="emailService"></param>
        public TransactionService(
            ApplicationContext applicationContext,
            ILogger<TransactionService> logger,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            IServiceScopeFactory serviceScopeFactory,
            IEmailService emailService)
        {
            Logger = logger;
            ApplicationContext = applicationContext;
            Mapper = mapper;
            StripeConfiguration.ApiKey = appSettings.Value.StripeApiKey;
            ScopeFactory = serviceScopeFactory;
            EmailService = emailService;
        }

        /// <inheritdoc />
        /// <exception cref="UserNotFoundException">Thrown when the user is not found</exception>
        /// <exception cref="Exception">Thrown if error</exception>
        /// <exception cref="EventNotFoundException">Thrown if the event is not found</exception>
        public async Task<TransactionViewModel> CreateIntent(CreateIntentBody createIntentBody)
        {
            var user = await ApplicationContext.Users
                .Include(x => x.PaymentMethods)
                .FirstOrDefaultAsync(x => x.Id == createIntentBody.UserId);

            var e = await ApplicationContext.Events.AsSplitQuery()
                .Include(x => x.Promos)
                .Include(x => x.Host)
                .Include(x => x.Tickets)
                .Include(x => x.Transactions)
                .FirstOrDefaultAsync(x => x.Id == createIntentBody.EventId);

            if (user == null)
            {
                Logger.LogInformation("User was not found");
                throw new UserNotFoundException();
            }

            // if the user doesn't have a customer id or payment method throw exception
            if (user.StripeCustomerId == null || !user.PaymentMethods.Any())
            {
                throw new Exception("Not enough stripe info");
            }

            if (e == null)
            {
                Logger.LogInformation("Event was not found");
                throw new EventNotFoundException();
            }

            // if the host of the event hasn't setup a bank account throw exception
            if (e.Host.StripeAccountId == null) throw new Exception("The host doesn't have enough stripe info");

            // gets a promo that's currently active
            var promo = e.Promos.FirstOrDefault(x => x.Active && x.Start < DateTime.Now && DateTime.Now < x.End);

            if (promo != null)
            {
                // applies discount to price
                createIntentBody.Amount -= (createIntentBody.Amount * promo.Discount / 100);
                Logger.LogInformation("Found promo for event");
            }

            // Stipee api create intent options
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
                Metadata = new Dictionary<string, string>
                {
                    {"EventId", e.Id.ToString()},
                    {"PromoId", promo?.Id.ToString()}
                },
            };

            var service = new PaymentIntentService();

            try
            {
                var ticket = e.Tickets.FirstOrDefault(x => x.Available);

                // if there's no tickets left then throw exception
                if (ticket == null) throw new Exception("Out of tickets");

                // Sends request to Strip api to create intent
                var intent = service.Create(options);

                e.TicketsLeft = e.Tickets.Count(x => x.Available) - 1;

                ticket.User = user;
                ticket.Available = false;

                // records transaction using Stripe response
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
                    ClientSecret = intent.ClientSecret,
                    PromoId = promo?.Id
                };

                await ApplicationContext.Transactions.AddAsync(transaction);
                await ApplicationContext.SaveChangesAsync();

                // start the intent timout method
                CheckOnIntent(transaction.StripeIntentId, ticket.Id, e.Id);

                return Mapper.Map<TransactionViewModel>(transaction);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        // time out check
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

        // Contacts Stripe, cancels intent and removes transaction
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

        /// <inheritdoc />
        /// <exception cref="UserNotFoundException">Thrown if the user is not found</exception>
        /// <exception cref="Exception">Thrown if error</exception>
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
            {
                throw new Exception("User doesn't own card");
            }

            if (transaction == null) throw new Exception("Transaction not found");

            var service = new PaymentIntentService();

            try
            {
                // request the to Stripe api with card id
                var intent = service.Update(injectPaymentMethodBody.IntentId,
                    new PaymentIntentUpdateOptions
                    {
                        PaymentMethod = injectPaymentMethodBody.CardId
                    });

                // update transaction using Stripe response
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

        /// <inheritdoc />
        /// <exception cref="Exception">Thrown if error</exception>
        public async Task<TransactionViewModel> ConfirmIntent(ConfirmIntentBody confirmIntentBody)
        {
            var transaction =
                await ApplicationContext.Transactions.FirstOrDefaultAsync(x =>
                    x.StripeIntentId == confirmIntentBody.IntentId);

            if (transaction == null) throw new Exception("Transaction not found");

            var service = new PaymentIntentService();

            try
            {
                // requests intent confirm from Stripe api
                var intent = service.Confirm(confirmIntentBody.IntentId);

                // updates transaction with Stripe response
                transaction.Updated = DateTime.Now;
                transaction.Status = Enum.Parse<PaymentStatus>(intent.Status, true);
                
                // if the intent has finished set paid
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
        
        /// <inheritdoc />
        public async Task CaptureIntentHook(Stripe.Event stripeEvent)
        {
            try
            {
                if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    Logger.LogInformation("Payment intent succeeded");

                    var stripeCharge = stripeEvent.Data.Object as PaymentIntent;

                    var transaction =
                        await ApplicationContext.Transactions.Include(x => x.User).AsSplitQuery()
                            .FirstOrDefaultAsync(x => x.StripeIntentId == stripeCharge.Id);

                    if (transaction != null)
                    {
                        transaction.Updated = DateTime.Now;
                        transaction.Status = PaymentStatus.succeeded;
                        transaction.Paid = true;
                        transaction.End = DateTime.Now;

                        await ApplicationContext.SaveChangesAsync();

                        // sends ticket purchase email if paid
                        await EmailService.SendAsync(transaction.User.Email,
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

                        // sends payment failure email if the payment failed
                        await EmailService.SendAsync(transaction.User.Email,
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
        
        /// <inheritdoc />
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