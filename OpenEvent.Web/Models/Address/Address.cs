using Microsoft.EntityFrameworkCore;

namespace OpenEvent.Web.Models.Address
{
    /// <summary>
    /// Address of event.
    /// </summary>
    [Owned]
    public class Address
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}