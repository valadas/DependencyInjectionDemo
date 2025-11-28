using DependencyInjectionDemo.Services.Payment.Providers;
using Microsoft.Extensions.Configuration;

namespace DependencyInjectionDemo.Services.Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;

        public PaymentService()
        {
            // Build configuration from appsettings.json
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        public bool ProcessPayment(decimal amount, string creditCardNumber)
        {
            // Get the active payment provider from configuration
            var activeProviderName = _configuration["PaymentSettings:ActiveProvider"];

            // Get all the payment providers through reflection of types that implement IPaymentProvider
            var paymentProviderTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(IPaymentProvider).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                .ToList();

            // Create instances of all available providers
            var availableProviders = paymentProviderTypes
                .Select(type => (IPaymentProvider)Activator.CreateInstance(type)!)
                .ToList();

            // Find the provider that matches the configured name
            var paymentProvider = availableProviders.FirstOrDefault(p =>
                p.Name.Equals(activeProviderName, StringComparison.OrdinalIgnoreCase));

            if (paymentProvider == null)
            {
                throw new InvalidOperationException(
                    $"Unknown payment provider: {activeProviderName}. Available providers: {string.Join(", ", availableProviders.Select(p => p.Name))}");
            }

            return paymentProvider.ProcessPayment(amount, creditCardNumber);
        }
    }
}
