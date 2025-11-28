using DependencyInjectionDemo.Services.Catalog.Data.Repositories;
using DependencyInjectionDemo.Services.Catalog.Data.ViewModels;
using System.Runtime.CompilerServices;

namespace DependencyInjectionDemo.Services.Catalog
{
    internal class CatalogService
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
