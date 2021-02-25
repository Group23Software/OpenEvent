using System;

namespace OpenEvent.Web.Models.PaymentMethod
{
    public class AddPaymentMethodBody
    {
       public Guid UserId { get; set; }
       public string CardToken { get; set; }
       public string NickName { get; set; }
    }
}