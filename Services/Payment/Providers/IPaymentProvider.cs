namespace DependencyInjectionDemo.Services.Payment.Providers
{
    public interface IPaymentProvider
    {
        string Name { get; }
        bool ProcessPayment(decimal amount, string creditCardNumber);
    }
}
