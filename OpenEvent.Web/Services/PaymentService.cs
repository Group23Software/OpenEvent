using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Exceptions;
using OpenEvent.Web.Models.PaymentMethod;
using OpenEvent.Web.Models.User;
using Stripe;
using PaymentMethod = OpenEvent.Web.Models.PaymentMethod.PaymentMethod;

namespace OpenEvent.Web.Services
{
    public interface IPaymentService
    {
        Task<PaymentMethodViewModel> AddPaymentMethod(AddPaymentMethodBody addPaymentMethodBody);
        Task MakeDefault(MakeDefaultBody makeDefaultBody);
        Task RemovePaymentMethod(RemovePaymentMethodBody removePaymentMethodBody);
    }

    public class PaymentService : IPaymentService
    {
        private readonly ILogger<PaymentService> Logger;
        private readonly ApplicationContext ApplicationContext;
        private readonly IMapper Mapper;

        public PaymentService(ApplicationContext applicationContext, ILogger<PaymentService> logger, IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            Logger = logger;
            ApplicationContext = applicationContext;
            Mapper = mapper;
            StripeConfiguration.ApiKey = appSettings.Value.StripeApiKey;
        }

        public async Task<PaymentMethodViewModel> AddPaymentMethod(AddPaymentMethodBody addPaymentMethodBody)
        {
            var user = await ApplicationContext.Users.Include(x => x.PaymentMethods)
                .FirstOrDefaultAsync(x => x.Id == addPaymentMethodBody.UserId);

            if (user == null)
            {
                throw new UserNotFoundException();
            }

            if (user.StripeCustomerId == null)
            {
                var customer = await CreatCustomer(user);
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
                var card = service.Create(user.StripeCustomerId, options);

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
                Logger.LogWarning(e.Message);
                throw;
            }
        }

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
                userWithPayments.PaymentMethods.ForEach(p =>
                {
                    p.IsDefault = p.StripeCardId == makeDefaultBody.PaymentId;
                });

                var options = new CustomerUpdateOptions()
                {
                    DefaultSource = makeDefaultBody.PaymentId
                };

                var service = new CustomerService();
                service.Update(userWithPayments.StripeCustomerId, options);
                
                await ApplicationContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Logger.LogWarning(e.Message);
                throw;
            }
        }

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
                service.Delete(userWithPayments.StripeCustomerId, paymentMethod.StripeCardId);
                ApplicationContext.PaymentMethods.Remove(paymentMethod);
                if (paymentWasDefault) userWithPayments.PaymentMethods.First().IsDefault = true;
                await ApplicationContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Logger.LogWarning(e.Message);
                throw;
            }
        }

        private async Task<Customer> CreatCustomer(User user)
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
                Logger.LogWarning(e.Message);
                throw;
            }
        }
    }
}