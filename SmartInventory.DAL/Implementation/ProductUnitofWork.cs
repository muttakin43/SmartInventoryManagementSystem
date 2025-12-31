using Microsoft.EntityFrameworkCore;
using SmartInventory.DAL.Core;
using SmartInventory.DAL.Interface;


namespace SmartInventory.DAL.Implementation
{
    public class ProductUnitofWork : UnitofWork, IProductUnitofWork
    {
        public ProductUnitofWork(DbContext context) : base(context)
        {
        }

        public IProductRepository ProductRepository => throw new NotImplementedException();
    }
}
