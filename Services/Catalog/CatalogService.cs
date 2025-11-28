using DependencyInjectionDemo.Services.Catalog.Data.Repositories;
using DependencyInjectionDemo.Services.Catalog.Data.ViewModels;

namespace DependencyInjectionDemo.Services.Catalog
{
    public class CatalogService
    {
        public List<ProductViewModel> GetAllProducts()
        {
            var repository = new ProductRepository();
            var products = repository.GetAllProducts();
            
            return products
                .Select(p => new ProductViewModel(p))
                .OrderBy(p => p.Id)
                .ToList();
        }
    }
}
