namespace OpenEvent.Web.Models.Auth
{
    /// <summary>
    /// Request body for user login.
    /// </summary>
    public class LoginBody
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Remember { get; set; }
    }
}