using System;

namespace OpenEvent.Web.Exceptions
{
    /// <summary>
    /// Invalid promo exception
    /// </summary>
    public class InvalidPromoException : Exception
    {
        /// <inheritdoc />
        public InvalidPromoException() : base("Invalid promo, conflicting dates!")
        {
        }
    }
}