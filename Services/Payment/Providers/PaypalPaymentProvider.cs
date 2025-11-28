namespace DependencyInjectionDemo.Services.Payment.Providers
{
    public class PaypalPaymentProvider : IPaymentProvider
    {
        public string Name => "PayPal";

        public bool ProcessPayment(decimal amount, string creditCardNumber)
        {
            // Simulate successful payment processing via PayPal.
            Console.WriteLine($"Processing payment using Paypal");
            return true;
        }
    }
}
