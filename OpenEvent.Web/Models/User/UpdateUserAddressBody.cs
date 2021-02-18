using System;

namespace OpenEvent.Web.Models.User
{
    public class UpdateUserAddressBody
    {
        public Guid Id { get; set; }
        public Address.Address Address { get; set; }
    }
}