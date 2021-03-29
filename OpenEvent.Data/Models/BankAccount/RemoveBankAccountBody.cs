using System;

namespace OpenEvent.Data.Models.BankAccount
{
    /// <summary>
    /// Request body for removing a bank account
    /// </summary>
    public class RemoveBankAccountBody
    {
        /// <summary>
        /// User's id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Bank's id
        /// </summary>
        public string BankId { get; set; }
    }
}