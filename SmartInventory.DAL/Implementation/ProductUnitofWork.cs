using Microsoft.EntityFrameworkCore;
using SmartInventory.DAL.Content;
using SmartInventory.DAL.Core;
using SmartInventory.DAL.Interface;


namespace SmartInventory.DAL.Implementation
{
    public class ProductUnitofWork : UnitofWork, IProductUnitofWork
    {
        public ProductUnitofWork(
            SmartInventoryDbContext context,
            IProductRepository productRepository) : base(context)
        {
            ProductRepository = productRepository;
        }

        public IProductRepository ProductRepository { get; }
    }
}
