
using DependencyInjectionDemo.Services.Catalog;
using DependencyInjectionDemo.Services.Catalog.Data.Repositories;
using DependencyInjectionDemo.Services.Payment;

namespace DependencyInjectionDemo.Services.Cart
{
    public class CartService
    {
        private readonly ICatalogService catalogService;
        private readonly IPaymentService paymentService;

        private readonly List<int> productIds;

        public CartService(
            ICatalogService catalogService,
            IPaymentService paymentService)
        {
            this.catalogService = catalogService;
            this.paymentService = paymentService;

            this.productIds = new List<int>();
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

            var success = paymentService.ProcessPayment(totalAmount, creditardNumber);

            return success;
        }
    }
}
