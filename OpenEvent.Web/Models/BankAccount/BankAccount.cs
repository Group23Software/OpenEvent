using System.ComponentModel.DataAnnotations;

namespace OpenEvent.Web.Models.BankAccount
{
    public class BankAccount
    {
        [Key]
        public string StripeBankAccountId { get; set; }
        public User.User User { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public string LastFour { get; set; }
        public string Bank { get; set; }
    }
}