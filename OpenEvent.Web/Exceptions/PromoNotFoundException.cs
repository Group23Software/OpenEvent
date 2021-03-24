using System;

namespace OpenEvent.Web.Exceptions
{
    /// <summary>
    /// Promo not found exception
    /// </summary>
    public class PromoNotFoundException : Exception
    {
        /// <inheritdoc />
        public PromoNotFoundException(): base("Promo not found")
        {
        }        
    }
}