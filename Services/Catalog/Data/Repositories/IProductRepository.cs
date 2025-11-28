using DependencyInjectionDemo.Services.Catalog.Data.Entiities;

namespace DependencyInjectionDemo.Services.Catalog.Data.Repositories
{
    /// <summary>
    /// Provides data access methods for products.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Retrieves a list of all products available in the data store.
        /// </summary>
        /// <returns>A list of <see cref="Product"/> objects representing all products. The list will be empty if no products are
        /// available.</returns>
        List<Product> GetAllProducts();
    }
}