using System;

namespace OpenEvent.Web.Models.User
{
    /// <summary>
    /// Request body for creating a new user
    /// </summary>
    public class NewUserBody
    {
        /// <summary>
        /// User's email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Avatar image stored in a byte array
        /// </summary>
        public byte[] Avatar { get; set; }

        /// <summary>
        /// Unique username
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Unique phone number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// User's date of birth with zeroed time
        /// </summary>
        public DateTime DateOfBirth { get; set; }
    }
}