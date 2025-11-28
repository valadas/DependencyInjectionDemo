
using DependencyInjectionDemo.Services.Catalog;
using DependencyInjectionDemo.Services.Catalog.Data.Repositories;
using DependencyInjectionDemo.Services.Payment;

namespace DependencyInjectionDemo.Services.Cart
{
    public class CartService
    {
        private readonly IProductRepository productRepository;
        
        private readonly List<int> productIds;

        public CartService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;

            this.productIds = new List<int>();
        }

        public void AddProduct(int productId)
        {
            this.productIds.Add(productId);
        }

        public bool Checkout(string creditardNumber)
        {
            // Get the total amount from the products in the cart.
            var catalogService = new CatalogService(this.productRepository);
            var products = catalogService.GetAllProducts()
                .Where(p => this.productIds.Contains(p.Id))
                .ToList();
            decimal totalAmount = products.Sum(p => p.Price);

            var paymentService = new PaymentService();
            var success = paymentService.ProcessPayment(totalAmount, creditardNumber);

            return success;
        }
    }
}
