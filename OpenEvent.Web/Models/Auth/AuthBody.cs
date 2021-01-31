namespace OpenEvent.Web.Models.Auth
{
    public class AuthBody
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Remember { get; set; }
    }
}