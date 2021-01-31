using System;

namespace OpenEvent.Web.Models.User
{
    // Full user model with all data
    public class User
    {
        public virtual Guid Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }

        public virtual string Email { get; set; }

        // TODO: Email confirm
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual byte[] Avatar { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string Token { get; set; }
        public virtual DateTime DateOfBirth { get; set; }
    }
}