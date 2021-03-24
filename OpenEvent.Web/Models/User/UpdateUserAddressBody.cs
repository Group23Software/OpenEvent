using System;

namespace OpenEvent.Web.Models.User
{
    /// <summary>
    /// Request body for updating a user's address
    /// </summary>
    public class UpdateUserAddressBody
    {
        /// <summary>
        /// User's id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Updated address
        /// </summary>
        public Address.Address Address { get; set; }
    }
}