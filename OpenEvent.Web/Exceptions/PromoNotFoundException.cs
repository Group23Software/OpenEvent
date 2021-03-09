using System;

namespace OpenEvent.Web.Exceptions
{
    public class PromoNotFoundException : Exception
    {
        public PromoNotFoundException(): base("Promo not found")
        {
        }        
    }
}