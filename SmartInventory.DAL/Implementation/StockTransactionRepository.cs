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
    public class StockTransactionRepository : Repository<StockTransaction,int,SmartInventoryDbContext>,IStockTransactionRepository
    {
        public StockTransactionRepository(SmartInventoryDbContext context ) : base( context ) 
        {
            
        }

        public async Task Addasync(StockTransaction entity)
        {
            await _context.StockTransactions.AddAsync(entity);
        }
    }
}
