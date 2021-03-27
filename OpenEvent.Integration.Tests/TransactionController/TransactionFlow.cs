using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using OpenEvent.Web.Models.Intent;
using OpenEvent.Web.Models.PaymentMethod;
using OpenEvent.Web.Models.Transaction;

namespace OpenEvent.Integration.Tests.TransactionController
{
    [TestFixture]
    public class TransactionFlow
    {
        private HttpClient Client;

        [SetUp]
        public void Setup()
        {
            Client = ClientFactory.Create();
        }

        [Test]
        public async Task Should_Flow_Transaction()
        {
            var loggedUser = await TestData.LogUserIn(Client);

            // Add payment method
            AddPaymentMethodBody addPaymentBody = new()
            {
                UserId = loggedUser.Id,
                NickName = "Debit",
                CardToken = "tok_gb_debit"
            };

            var response = await Client.PostAsJsonAsync(TestData.BaseUrl + "/api/payment/AddPaymentMethod", addPaymentBody);
            response.StatusCode.Should().Be(200);

            var methodString = await response.Content.ReadAsStringAsync();
            var method = JsonConvert.DeserializeObject<PaymentMethodViewModel>(methodString);

            // create intent
            CreateIntentBody body = new()
            {
                UserId = loggedUser.Id,
                Amount = 1000,
                EventId = TestData.Event.Id
            };

            response = await Client.PostAsJsonAsync(TestData.BaseUrl + "/api/transaction/CreateIntent", body);
            response.StatusCode.Should().Be(200);

            var transactionString = await response.Content.ReadAsStringAsync();
            var transaction = JsonConvert.DeserializeObject<Transaction>(transactionString);

            transaction.Should().NotBeNull();
            transaction.Status.Should().Be(PaymentStatus.requires_payment_method);

            // inject payment method
            InjectPaymentMethodBody injectPaymentMethodBody = new()
            {
                IntentId = transaction.StripeIntentId,
                UserId = loggedUser.Id,
                CardId = method.StripeCardId
            };

            response = await Client.PostAsJsonAsync(TestData.BaseUrl + "/api/transaction/InjectPaymentMethod", injectPaymentMethodBody);
            response.StatusCode.Should().Be(200);

            transactionString = await response.Content.ReadAsStringAsync();
            transaction = JsonConvert.DeserializeObject<Transaction>(transactionString);

            transaction.Should().NotBeNull();
            transaction.Status.Should().Be(PaymentStatus.requires_confirmation);

            // confirm intent
            ConfirmIntentBody confirmIntentBody = new()
            {
                IntentId = transaction.StripeIntentId,
                UserId = loggedUser.Id
            };

            response = await Client.PostAsJsonAsync(TestData.BaseUrl + "/api/transaction/ConfirmIntent", confirmIntentBody);
            response.StatusCode.Should().Be(200);

            transactionString = await response.Content.ReadAsStringAsync();
            transaction = JsonConvert.DeserializeObject<Transaction>(transactionString);

            transaction.Should().NotBeNull();
            (transaction.Status == PaymentStatus.succeeded || transaction.Status == PaymentStatus.processing).Should().BeTrue();
        }
    }
}