namespace OpenEvent.Web
{
    /// <summary>
    /// App config options.
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// JWT secret.
        /// </summary>
        public string Secret { get; init; }

        /// <summary>
        /// Remote connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Azure maps api key for getting cords
        /// </summary>
        public string AzureMapsKey { get; set; }

        /// <summary>
        /// Stripe api key for payment processing
        /// </summary>
        public string StripeApiKey { get; set; }

        /// <summary>
        /// Url of app
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Required Email config for sending mail
        /// </summary>
        public MailSettings MailSettings { get; set; }
    }

    /// <summary>
    /// Email settings
    /// </summary>
    public class MailSettings
    {
        /// <summary>
        /// Smtp server
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Port
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Sender's displayed name
        /// </summary>
        public string SenderName { get; set; }

        /// <summary>
        /// Email address of sender
        /// </summary>
        public string SenderEmail { get; set; }

        /// <summary>
        /// Email Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Email password
        /// </summary>
        public string Password { get; set; }
    }
}