using System;

namespace OpenEvent.Web.Models.BankAccount
{
    public class AddBankAccountBody
    {
        public Guid UserId { get; set; }
        public string BankToken { get; set; }
    }
}