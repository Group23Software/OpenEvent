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
        /// Local connection string.
        /// </summary>
        public string LocalConnectionString { get; set; }
        public string AzureMapsKey { get; set; }
        
        public string StripeApiKey { get; set; }
        
        public string BaseUrl { get; set; }

        public MailSettings MailSettings { get; set; }
    }

    public class MailSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string SenderName { get; set; }
        public string SenderEmail { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}