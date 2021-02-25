using System;

namespace OpenEvent.Web.Models.BankAccount
{
    public class RemoveBankAccountBody
    {
        public Guid UserId { get; set; }
        public string BankId { get; set; }
    }
}