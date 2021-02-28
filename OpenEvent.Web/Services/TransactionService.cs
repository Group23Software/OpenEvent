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
using Serilog;
using Stripe;

namespace OpenEvent.Web.Services
{
    public interface ITransactionService
    {
        Task<PaymentIntent> CreateIntent(CreateIntentBody createIntentBody);
        Task<PaymentIntent> InjectPaymentMethod(InjectPaymentMethodBody injectPaymentMethodBody);
        Task CaptureIntentHook(); // Called by payment intent webhook
    }
    
    public class TransactionService : ITransactionService
    {
        private readonly ILogger<TransactionService> Logger;
        private readonly ApplicationContext ApplicationContext;
        private readonly IMapper Mapper;
        
        public TransactionService(ApplicationContext applicationContext, ILogger<TransactionService> logger, IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            Logger = logger;
            ApplicationContext = applicationContext;
            Mapper = mapper;
            StripeConfiguration.ApiKey = appSettings.Value.StripeApiKey;
        }
        
        public async Task<PaymentIntent> CreateIntent(CreateIntentBody createIntentBody)
        {
            var user = await ApplicationContext.Users.Include(x => x.PaymentMethods).FirstOrDefaultAsync(x => x.Id == createIntentBody.UserId);
            var e = await ApplicationContext.Events.Include(x => x.Host).Include(x => x.Tickets).FirstOrDefaultAsync(x => x.Id == createIntentBody.EventId);

            if (user == null)
            {
                Logger.LogInformation("User was not found");
                throw new UserNotFoundException();
            }

            if (user.StripeCustomerId == null || !user.PaymentMethods.Any()) throw new Exception("Not enough stripe info");

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

            var intent = service.Create(options);

            return intent;
        }

        public async Task<PaymentIntent> InjectPaymentMethod(InjectPaymentMethodBody injectPaymentMethodBody)
        {
            var user = await ApplicationContext.Users.FirstOrDefaultAsync(x => x.Id == injectPaymentMethodBody.UserId);
            
            if (user == null)
            {
                Logger.LogInformation("User was not found");
                throw new UserNotFoundException();
            }

            if (user.PaymentMethods.All(x => x.StripeCardId != injectPaymentMethodBody.CardId)) throw new Exception("User doesn't own card");

            var service = new PaymentIntentService();
            
            var paymentConfirmation = await service.ConfirmAsync(injectPaymentMethodBody.IntentId,new PaymentIntentConfirmOptions()
            {
                PaymentMethod = injectPaymentMethodBody.CardId

            });

            return paymentConfirmation;
        }

        public Task CaptureIntentHook()
        {
            throw new System.NotImplementedException();
        }
    }
}