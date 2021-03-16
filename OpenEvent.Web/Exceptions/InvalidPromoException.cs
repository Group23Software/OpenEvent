using System;

namespace OpenEvent.Web.Exceptions
{
    public class InvalidPromoException : Exception
    {
        /// <inheritdoc />
        public InvalidPromoException() : base("Invalid promo, conflicting dates!")
        {
        }
    }
}