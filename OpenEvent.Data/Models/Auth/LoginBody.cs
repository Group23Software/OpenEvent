namespace OpenEvent.Data.Models.Auth
{
    /// <summary>
    /// Request body for user login.
    /// </summary>
    public class LoginBody
    {
        /// <summary>
        /// User's email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User's password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// If the token should last 30 days
        /// </summary>
        public bool Remember { get; set; }
    }
}