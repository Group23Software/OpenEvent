using System;

namespace OpenEvent.Web.Models.User
{
    /// <summary>
    /// Data collected when making a new user.
    /// </summary>
    public class NewUserInput
    {
        public string Email { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public byte[] Avatar { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}