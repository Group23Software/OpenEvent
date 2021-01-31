using System;

namespace OpenEvent.Web.Models.User
{
    // Data collected when making a new user
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

        public bool Remember { get; set; }
    }
}