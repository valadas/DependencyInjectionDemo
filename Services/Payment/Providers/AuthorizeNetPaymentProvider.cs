namespace DependencyInjectionDemo.Services.Payment.Providers
{
    internal class AuthorizeNetPaymentProvider : IPaymentProvider
    {
        public string Name => "AuthorizeNet";

        public bool ProcessPayment(decimal amount, string creditCardNumber)
        {
            // Client does not yet have it setup, to implement later.
            throw new NotImplementedException();
        }
    }
}
