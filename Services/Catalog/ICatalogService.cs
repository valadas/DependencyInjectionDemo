using DependencyInjectionDemo.Services.Catalog.Data.ViewModels;

namespace DependencyInjectionDemo.Services.Catalog
{
    /// <summary>
    /// Provides catalog services.
    /// </summary>
    public interface ICatalogService
    {
        /// <summary>
        /// Retrieves a list of all available products for display.
        /// </summary>
        /// <returns>A list of <see cref="ProductViewModel"/> instances representing all products. The list will be empty if no
        /// products are available.</returns>
        List<ProductViewModel> GetAllProducts();
    }
}