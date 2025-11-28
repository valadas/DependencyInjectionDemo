using DependencyInjectionDemo.Services.Cart;
using DependencyInjectionDemo.Services.Catalog;

namespace DependencyInjectionDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Show the products from the catalog
            var catalogService = new CatalogService();
            var products = catalogService.GetAllProducts();
            foreach (var product in products)
            {
                Console.WriteLine($"ID: {product.Id} Product: {product.Name}, Price: {product.Price}");
            }

            // Ask user to choose which product to buy.
            Console.WriteLine("Enter the product ID to buy:");
            var input = Console.ReadLine();

            // Add the selected product to the cart.
            var cartService = new CartService();
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
            Console.ReadLine();
        }
    }
}
