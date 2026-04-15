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
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IPurchaseDetailsRepository, PurchaseDetailsRepository>();
            services.AddScoped<IPurchaseRepository, PurchaseRepository>();
            services.AddScoped<IStockTransactionRepository, StockTransactionRepository>(); 
            services.AddScoped<ISaleRepository, SaleRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            
            
            
            services.AddScoped<IProductUnitofWork, ProductUnitofWork>();
            services.AddScoped<ISupplierUnitOfWork, SupplierUnitOfWork>();
            services.AddScoped<ICategoryUnitOfWork, CategoryUnitOfWork>();
            services.AddScoped<IPurchaseUnitOfWork, PurchaseUnitOfWork>();
            services.AddScoped<ISaleUnitOfWork, SaleUnitOfWork>();

            return services;
        }
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // Register Services here
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IPurchaseService, PurchaseService>();
            services.AddScoped<ISaleService, SaleService>();
            services.AddScoped<IStockTransactionService, StockTransactionService>();
            services.AddScoped<ICustomerService, CustomerService>();
            

            return services;
        }
    }
}
