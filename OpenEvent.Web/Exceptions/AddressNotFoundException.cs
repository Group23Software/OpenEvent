using System;

namespace OpenEvent.Web.Exceptions
{
    public class AddressNotFoundException : Exception
    {
        public AddressNotFoundException() : base("Address Not Found")
        {
        }
    }
}