using System.Threading.Tasks;
using OpenEvent.Data.Models.PaymentMethod;

namespace OpenEvent.Web.Services
{
    /// <summary>
    /// Service for all payment method logic
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// Creates a stripe customer if null.
        /// Adds card token to stripe customer.
        /// Adds card to user.
        /// </summary>
        /// <param name="addPaymentMethodBody"></param>
        /// <returns>Returns the newly added payment method</returns>
        Task<PaymentMethodViewModel> AddPaymentMethod(AddPaymentMethodBody addPaymentMethodBody);

        /// <summary>
        /// Sets card to default.
        /// Updates stripe default.
        /// </summary>
        /// <param name="makeDefaultBody"></param>
        /// <returns>Returns completed task once the card is made default</returns>
        Task MakeDefault(MakeDefaultBody makeDefaultBody);

        /// <summary>
        /// Deletes card from stripe customer.
        /// Removes card from user.
        /// </summary>
        /// <param name="removePaymentMethodBody"></param>
        /// <returns>Returns completed task one the payment method has been removed</returns>
        Task RemovePaymentMethod(RemovePaymentMethodBody removePaymentMethodBody);
    }
}