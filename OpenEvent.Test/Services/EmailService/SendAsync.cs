using System.Threading.Tasks;
using NUnit.Framework;
using OpenEvent.Test.Factories;

namespace OpenEvent.Test.Services.EmailService
{
    [TestFixture]
    public class SendAsync
    {
        [Test]
        [Ignore("Ignore")]
        public async Task Should_Send_Purchase_Confirmation()
        {
            var service = new EmailServiceFactory().Create();
            await service.SendAsync("harrison@thebarkers.me.uk", "<h1>Ticket Purchased</h1><p>You have successfully purchased a ticket</p>","Subject");
        }
    }
}