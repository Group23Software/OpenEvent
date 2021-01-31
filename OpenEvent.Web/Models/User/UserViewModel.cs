using System;

namespace OpenEvent.Web.Models.User
{
    // Basic user data returned when authenticating the user
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public string Token { get; set; }
    }
}