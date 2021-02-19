using System;

namespace OpenEvent.Web.Exceptions
{
    public class BankAccountNotFoundException : Exception
    {
        public BankAccountNotFoundException() : base("Bank account not found")
        {
        }
    }
}