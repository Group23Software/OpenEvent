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
    }
}