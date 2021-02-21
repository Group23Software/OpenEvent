using System;

namespace OpenEvent.Web.Exceptions
{
    /// <summary>
    /// Bank account not found exception.
    /// </summary>
    public class BankAccountNotFoundException : Exception
    {
        /// <inheritdoc />
        public BankAccountNotFoundException() : base("Bank account not found")
        {
        }
    }
}