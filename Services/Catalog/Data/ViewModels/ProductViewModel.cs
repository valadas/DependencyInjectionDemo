using DependencyInjectionDemo.Services.Catalog.Data.Entiities;

namespace DependencyInjectionDemo.Services.Catalog.Data.ViewModels
{
    public class ProductViewModel
    {
        public ProductViewModel(Product product)
        {
            this.Id = product.Id;
            this.Name = product.Name;
            this.Price = product.Price;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
