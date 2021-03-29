using System.Threading.Tasks;
using OpenEvent.Data.Models.BankAccount;

namespace OpenEvent.Web.Services
{
    /// <summary>
    /// Service providing all banking logic.
    /// </summary>
    public interface IBankingService
    {
        /// <summary>
        /// Creates a stripe account if one does not exist.
        /// Adds bank token to the user's stripe account.
        /// Adds bank account to the user entity.
        /// </summary>
        /// <param name="addBankAccountBody"><see cref="AddBankAccountBody"/></param>
        /// <returns>Returns newly created bank account</returns>
        Task<BankAccountViewModel> AddBankAccount(AddBankAccountBody addBankAccountBody);
        /// <summary>
        /// Deletes the users stripe account.
        /// </summary>
        /// <param name="removeBankAccountBody"></param>
        /// <returns>Completed task once the bank account has been removed</returns>
        Task RemoveBankAccount(RemoveBankAccountBody removeBankAccountBody);
    }
}