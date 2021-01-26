using Microsoft.AspNetCore.Identity;

namespace OpenEvent.Web.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] Avatar { get; set; }
    }
}