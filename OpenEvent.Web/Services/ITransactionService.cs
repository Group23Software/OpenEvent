using System.Threading.Tasks;
using OpenEvent.Data.Models.Intent;
using OpenEvent.Data.Models.Transaction;

namespace OpenEvent.Web.Services
{
    /// <summary>
    /// Service implementing all methods needed for payment processing
    /// </summary>
    public interface ITransactionService
    {
        /// <summary>
        /// Method that creates a stripe intent and Open Event transaction
        /// </summary>
        /// <param name="createIntentBody"></param>
        /// <returns>
        /// Returns the current transaction
        /// </returns>
        Task<TransactionViewModel> CreateIntent(CreateIntentBody createIntentBody);

        /// <summary>
        /// Injects the users payment method into the payment intent
        /// </summary>
        /// <param name="injectPaymentMethodBody"><see cref="InjectPaymentMethodBody"/></param>
        /// <returns>
        /// Returns the current transaction
        /// </returns>
        Task<TransactionViewModel> InjectPaymentMethod(InjectPaymentMethodBody injectPaymentMethodBody);

        /// <summary>
        /// Confirms the intent to finish the transaction
        /// </summary>
        /// <param name="confirmIntentBody"></param>
        /// <returns>
        /// Returns the current transaction
        /// </returns>
        Task<TransactionViewModel> ConfirmIntent(ConfirmIntentBody confirmIntentBody);

        /// <summary>
        /// Method called by Stripe intent webhook, sends email if payment succeeded or failed
        /// </summary>
        /// <param name="stripeEvent">Stripe intent event</param>
        /// <returns>Completed task once event has been dealt with</returns>
        Task CaptureIntentHook(Stripe.Event stripeEvent);

        /// <summary>
        /// Cancels Stripe intent
        /// </summary>
        /// <param name="cancelIntentBody"></param>
        /// <returns>
        /// Completed task once finished
        /// </returns>
        Task CancelIntent(CancelIntentBody cancelIntentBody);
    }
}