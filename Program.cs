using DependencyInjectionDemo.Services.Cart;
using DependencyInjectionDemo.Services.Catalog;
using DependencyInjectionDemo.Services.Catalog.Data.Repositories;
using DependencyInjectionDemo.Services.Payment;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Build configuration from appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Setup Dependency Injection container
            var services = new ServiceCollection();
            
            // Register configuration
            services.AddSingleton<IConfiguration>(configuration);
            
            // Register application services
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICatalogService, CatalogService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<ICartService, CartService>();

            // Build the service provider
            var serviceProvider = services.BuildServiceProvider();

            // Create a scope for the request/operation
            using (var scope = serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;

                // Resolve services from the scoped container
                var cartService = scopedServices.GetRequiredService<ICartService>();
                var catalogService = scopedServices.GetRequiredService<ICatalogService>();

                // Show the products from the catalog
                var products = catalogService.GetAllProducts();
                foreach (var product in products)
                {
                    Console.WriteLine($"ID: {product.Id} Product: {product.Name}, Price: {product.Price}");
                }

                // Ask user to choose which product to buy.
                Console.WriteLine("Enter the product ID to buy:");
                var input = Console.ReadLine();

                // Add the selected product to the cart.
                if (int.TryParse(input, out int productId))
                {
                    cartService.AddProduct(productId);
                }
                else
                {
                    Console.WriteLine("Invalid product ID.");
                }

                // Ask user for credit card number.
                Console.WriteLine("Enter your credit card number to checkout:");
                var cardNumber = Console.ReadLine();

                // Pay for the orders in the cart.
                cartService.Checkout(cardNumber!);

                Console.WriteLine("Thank you for your purchase!");
            } // Scope is disposed here, all scoped services are cleaned up

            Console.ReadLine();
        }
    }
}
