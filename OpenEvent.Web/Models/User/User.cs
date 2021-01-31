using System;

namespace OpenEvent.Web.Models.User
{
    /// <summary>
    /// Full user model with all data.
    /// </summary>
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }

        // TODO: Email confirm
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] Avatar { get; set; }
        public string PhoneNumber { get; set; }
        public string Token { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}