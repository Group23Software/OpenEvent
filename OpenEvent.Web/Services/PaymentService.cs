using System;
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
        Task<PaymentMethodViewModel> AddPaymentMethod(AddPaymentMethodModel addPaymentMethodModel);
    }

    public class PaymentService : IPaymentService
    {
        private readonly ILogger<PaymentService> Logger;
        private readonly ApplicationContext ApplicationContext;

        private readonly IMapper Mapper;
        // private readonly AppSettings AppSettings;

        public PaymentService(ApplicationContext applicationContext, ILogger<PaymentService> logger, IMapper mapper,
            IOptions<AppSettings> appSettings)
        {
            Logger = logger;
            ApplicationContext = applicationContext;
            Mapper = mapper;
            StripeConfiguration.ApiKey = appSettings.Value.StripeApiKey;
        }

        public async Task<PaymentMethodViewModel> AddPaymentMethod(AddPaymentMethodModel addPaymentMethodModel)
        {
            var user = await ApplicationContext.Users.Include(x => x.PaymentMethods).FirstOrDefaultAsync(x => x.Id == addPaymentMethodModel.UserId);

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
                Source = addPaymentMethodModel.CardToken
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
                    NickName = addPaymentMethodModel.NickName,
                    IsDefault = false
                };

                try
                {
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