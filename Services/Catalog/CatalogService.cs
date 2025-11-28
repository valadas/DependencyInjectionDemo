using DependencyInjectionDemo.Services.Catalog.Data.Repositories;
using DependencyInjectionDemo.Services.Catalog.Data.ViewModels;

namespace DependencyInjectionDemo.Services.Catalog
{
    public class CatalogService : ICatalogService
    {
        private readonly IProductRepository productRepository;

        public CatalogService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public List<ProductViewModel> GetAllProducts()
        {
            var products = this.productRepository.GetAllProducts();

            return products
                .Select(p => new ProductViewModel(p))
                .OrderBy(p => p.Id)
                .ToList();
        }
    }
}
