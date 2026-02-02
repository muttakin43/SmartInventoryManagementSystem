using SmartInventory.BLL.Implementation;
using SmartInventory.BLL.Inteface;
using SmartInventory.DAL.Implementation;
using SmartInventory.DAL.Interface;

namespace SmartInventory.web
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            // Register repositories here
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductUnitofWork, ProductUnitofWork>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryUnitOfWork, CategoryUnitOfWork>();
            return services;
        }
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // Register Services here
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();

            return services;
        }
    }
}
