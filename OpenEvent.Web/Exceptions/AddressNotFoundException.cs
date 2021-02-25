using System;

namespace OpenEvent.Web.Exceptions
{
    /// <summary>
    /// Address not found exception.
    /// </summary>
    public class AddressNotFoundException : Exception
    {
        /// <inheritdoc />
        public AddressNotFoundException() : base("Address Not Found")
        {
        }
    }
}