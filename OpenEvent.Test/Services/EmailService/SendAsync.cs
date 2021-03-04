using System.Threading.Tasks;
using NUnit.Framework;

namespace OpenEvent.Test.Services.EmailService
{
    [TestFixture]
    public class SendAsync : EmailTestFixture
    {
        [Test]
        [Ignore("Ignore")]
        public async Task Should_Send_Purchase_Confirmation()
        {
            await EmailService.SendAsync("harrison@thebarkers.me.uk", "OpenEvent", "<h1>Ticket Purchased</h1><p>You have successfully purchased a ticket</p>","Subject");
        }
    }
}