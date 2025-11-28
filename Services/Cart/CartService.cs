
using DependencyInjectionDemo.Services.Catalog;
using DependencyInjectionDemo.Services.Catalog.Data.Repositories;
using DependencyInjectionDemo.Services.Payment;

namespace DependencyInjectionDemo.Services.Cart
{
    public class CartService
    {
        private readonly IProductRepository productRepository;
        private readonly ICatalogService catalogService;

        private readonly List<int> productIds;

        public CartService(
            IProductRepository productRepository,
            ICatalogService catalogService)
        {
            this.productRepository = productRepository;
            this.catalogService = catalogService;

            this.productIds = new List<int>();
            this.catalogService = catalogService;
        }

        public void AddProduct(int productId)
        {
            this.productIds.Add(productId);
        }

        public bool Checkout(string creditardNumber)
        {
            // Get the total amount from the products in the cart.
            var products = this.catalogService.GetAllProducts()
                .Where(p => this.productIds.Contains(p.Id))
                .ToList();
            decimal totalAmount = products.Sum(p => p.Price);

            var paymentService = new PaymentService();
            var success = paymentService.ProcessPayment(totalAmount, creditardNumber);

            return success;
        }
    }
}
