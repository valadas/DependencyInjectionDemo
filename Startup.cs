using DependencyInjectionDemo.Services.Cart;
using DependencyInjectionDemo.Services.Catalog;
using DependencyInjectionDemo.Services.Catalog.Data.Repositories;
using DependencyInjectionDemo.Services.Payment;
using Microsoft.Extensions.DependencyInjection;

namespace DependencyInjectionDemo
{
    internal class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICatalogService, CatalogService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<ICartService, CartService>();
        }
    }
}
