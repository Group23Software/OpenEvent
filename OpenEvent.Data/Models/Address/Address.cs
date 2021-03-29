using Microsoft.EntityFrameworkCore;

namespace OpenEvent.Data.Models.Address
{
    /// <summary>
    /// Simple address model
    /// </summary>
    [Owned]
    public class Address
    {
        /// <summary>
        /// House number, street
        /// </summary>
        public string AddressLine1 { get; set; }

        /// <summary>
        /// Apt, unit
        /// </summary>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Postcode
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Two letter country code
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Full Country name
        /// </summary>
        public string CountryName { get; set; }

        /// <summary>
        /// Latitude
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// Longitude
        /// </summary>
        public double Lon { get; set; }
    }
}