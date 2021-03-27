using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using OpenEvent.Web.Models.PaymentMethod;

namespace OpenEvent.Integration.Tests.PaymentController
{
    [TestFixture]
    public class AddAndRemove
    {
        private HttpClient Client;

        [SetUp]
        public void Setup()
        {
            Client = ClientFactory.Create();
        }

        [Test]
        public async Task Should_Add_And_Remove_Payment_Method()
        {
            var loggedUser = await TestData.LogUserIn(Client);

            AddPaymentMethodBody body = new AddPaymentMethodBody()
            {
                UserId = loggedUser.Id,
                NickName = "Debit",
                CardToken = "tok_gb_debit"
            };

            var response = await Client.PostAsJsonAsync(TestData.BaseUrl + "/api/payment/AddPaymentMethod",body);
            response.StatusCode.Should().Be(200);

            var methodString = await response.Content.ReadAsStringAsync();
            var method = JsonConvert.DeserializeObject<PaymentMethodViewModel>(methodString);

            RemovePaymentMethodBody removeBody = new()
            {
                PaymentId = method.StripeCardId,
                UserId = loggedUser.Id
            };
            
            response = await Client.PostAsJsonAsync(TestData.BaseUrl + "/api/payment/RemovePaymentMethod",removeBody);
            response.StatusCode.Should().Be(200);
        }
    }
}