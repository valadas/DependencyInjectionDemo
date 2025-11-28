namespace DependencyInjectionDemo.Services.Payment
{
    /// <summary>
    /// A service for processing payments.
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// Attempts to process a payment using the specified credit card and amount.
        /// </summary>
        /// <param name="amount">The monetary amount to be charged to the credit card. Must be a positive value.</param>
        /// <param name="creditCardNumber">The credit card number to be charged. Cannot be null or empty. Must be a valid credit card number format.</param>
        /// <returns>true if the payment was successfully processed; otherwise, false.</returns>
        bool ProcessPayment(decimal amount, string creditCardNumber);
    }
}