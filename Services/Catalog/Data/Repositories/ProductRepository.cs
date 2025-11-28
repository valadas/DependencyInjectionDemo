using DependencyInjectionDemo.Services.Catalog.Data.Entiities;

namespace DependencyInjectionDemo.Services.Catalog.Data.Repositories
{
    /// <inheritdoc cref="IProductRepository"/>
    internal class ProductRepository : IProductRepository
    {
        private List<Product> products;

        public ProductRepository()
        {
            this.products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 999.99m },
                new Product { Id = 2, Name = "Smartphone", Price = 499.99m },
                new Product { Id = 3, Name = "Tablet", Price = 299.99m }
            };
        }

        /// <inheritdoc/>
        public List<Product> GetAllProducts()
        {
            return this.products;
        }
    }
}
