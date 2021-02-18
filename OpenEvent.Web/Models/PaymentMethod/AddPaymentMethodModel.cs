using System;

namespace OpenEvent.Web.Models.PaymentMethod
{
    public class AddPaymentMethodModel
    {
       public Guid UserId { get; set; }
       public string CardToken { get; set; }
       public string NickName { get; set; }
    }
}