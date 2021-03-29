using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenEvent.Data.Models.PaymentMethod;
using OpenEvent.Data.Models.User;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Exceptions;
using Stripe;
using PaymentMethod = OpenEvent.Data.Models.PaymentMethod.PaymentMethod;

namespace OpenEvent.Web.Services
{
    /// <inheritdoc />
    public class PaymentService : IPaymentService
    {
        private readonly ILogger<PaymentService> Logger;
        private readonly ApplicationContext ApplicationContext;
        private readonly IMapper Mapper;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="applicationContext"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="appSettings"></param>
        public PaymentService(ApplicationContext applicationContext, ILogger<PaymentService> logger, IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            Logger = logger;
            ApplicationContext = applicationContext;
            Mapper = mapper;
            StripeConfiguration.ApiKey = appSettings.Value.StripeApiKey;
        }
        
        /// <inheritdoc />
        /// <exception cref="UserNotFoundException">Thrown when the user is not found</exception>
        public async Task<PaymentMethodViewModel> AddPaymentMethod(AddPaymentMethodBody addPaymentMethodBody)
        {
            var user = await ApplicationContext.Users.Include(x => x.PaymentMethods)
                .FirstOrDefaultAsync(x => x.Id == addPaymentMethodBody.UserId);

            if (user == null)
            {
                throw new UserNotFoundException();
            }

            // if the user doesn't have a stripe customer create one
            if (user.StripeCustomerId == null)
            {
                var customer = CreateCustomer(user);
                user.StripeCustomerId = customer.Id;
                await ApplicationContext.SaveChangesAsync();
            }

            var options = new CardCreateOptions()
            {
                Source = addPaymentMethodBody.CardToken
            };

            var service = new CardService();

            try
            {
                // request create card to Stripe api
                var card = service.Create(user.StripeCustomerId, options);

                // create payment method using Stripe response data
                var paymentMethod = new PaymentMethod()
                {
                    StripeCardId = card.Id,
                    Brand = card.Brand,
                    Country = card.Country,
                    Funding = card.Funding,
                    Name = card.Name,
                    ExpiryMonth = card.ExpMonth,
                    ExpiryYear = card.ExpYear,
                    LastFour = card.Last4,
                    NickName = addPaymentMethodBody.NickName,
                    IsDefault = !user.PaymentMethods.Any()
                };

                user.PaymentMethods.Add(paymentMethod);
                await ApplicationContext.SaveChangesAsync();
                
                return Mapper.Map<PaymentMethodViewModel>(paymentMethod);
            }
            catch (Exception e)
            {
                Logger.LogInformation(e.Message);
                throw;
            }
        }

        /// <inheritdoc />
        /// <exception cref="UserNotFoundException">Thrown if the user is not found</exception>
        public async Task MakeDefault(MakeDefaultBody makeDefaultBody)
        {
            var userWithPayments =
                await ApplicationContext.Users.Include(x => x.PaymentMethods)
                    .FirstOrDefaultAsync(x => x.Id == makeDefaultBody.UserId);

            if (userWithPayments == null)
            {
                throw new UserNotFoundException();
            }

            try
            {
                // set all user's payment methods to not default
                userWithPayments.PaymentMethods.ForEach(p =>
                {
                    p.IsDefault = p.StripeCardId == makeDefaultBody.PaymentId;
                });

                var options = new CustomerUpdateOptions()
                {
                    DefaultSource = makeDefaultBody.PaymentId
                };

                var service = new CustomerService();
                
                // request customer update to Stripe api
                service.Update(userWithPayments.StripeCustomerId, options);

                await ApplicationContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Logger.LogInformation(e.Message);
                throw;
            }
        }

        /// <inheritdoc />
        /// <exception cref="UserNotFoundException">Thrown when the user is not found</exception>
        /// <exception cref="PaymentMethodNotFoundException">Thrown when the users payment method was not found</exception>
        public async Task RemovePaymentMethod(RemovePaymentMethodBody removePaymentMethodBody)
        {
            var userWithPayments =
                await ApplicationContext.Users.Include(x => x.PaymentMethods)
                    .FirstOrDefaultAsync(x => x.Id == removePaymentMethodBody.UserId);

            if (userWithPayments == null)
            {
                throw new UserNotFoundException();
            }

            var paymentMethod =
                userWithPayments.PaymentMethods.FirstOrDefault(
                    x => x.StripeCardId == removePaymentMethodBody.PaymentId);

            if (paymentMethod == null)
            {
                throw new PaymentMethodNotFoundException();
            }

            bool paymentWasDefault = paymentMethod.IsDefault;

            var service = new CardService();

            try
            {
                // request card delete from the Stripe api
                service.Delete(userWithPayments.StripeCustomerId, paymentMethod.StripeCardId);
                
                // remove the payment method and set first to default if the removed card was the default
                ApplicationContext.PaymentMethods.Remove(paymentMethod);
                if (paymentWasDefault) userWithPayments.PaymentMethods.First().IsDefault = true;
                
                await ApplicationContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Logger.LogInformation(e.Message);
                throw;
            }
        }

        // Contacts the Stripe api to create a new customer using the user's details
        private Customer CreateCustomer(User user)
        {
            var options = new CustomerCreateOptions()
            {
                Email = user.Email,
                Phone = user.PhoneNumber,
                Name = user.FirstName + " " + user.LastName
            };
            var service = new CustomerService();

            try
            {
                var customer = service.Create(options);
                return customer;
            }
            catch (Exception e)
            {
                Logger.LogInformation(e.Message);
                throw;
            }
        }
    }
}