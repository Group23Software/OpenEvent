namespace OpenEvent.Web.Models.Transaction
{
    public enum PaymentStatus
    {
        canceled,
        processing,
        requires_action,
        requires_capture,
        requires_confirmation,
        requires_payment_method,
        succeeded
    }
}