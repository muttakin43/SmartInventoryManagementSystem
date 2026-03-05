using Microsoft.EntityFrameworkCore;
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
    public class SupplierRepository : Repository<Supplier, int,SmartInventoryDbContext>,
        ISupplierRepository
    {
        public SupplierRepository(SmartInventoryDbContext context) : base(context)
        {
        }
      
        
    
public async Task<int> CountSuppliersAsync()
        {
            return await _dbSet.CountAsync();
        }
    }
}
