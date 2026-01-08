using SmartInventory.DAL.Content;
using SmartInventory.DAL.Interface;
using SmartInventory.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.DAL.Implementation
{
    public class ProductRepository : Repository<Product, int, SmartInventoryDbContext>,
        IProductRepository
    {
        public ProductRepository(SmartInventoryDbContext context) : base(context)
        {
        }

     

        public int CountProduct()
        {
            return _dbSet.Count();
        }
     

     
    }
}
