using Microsoft.EntityFrameworkCore;
using SmartInventory.DAL.Content;
using SmartInventory.DAL.Core;
using SmartInventory.DAL.Interface;
using SmartInventory.Model;

namespace SmartInventory.DAL.Implementation
{

    public class SaleRepository
        : Repository<Sale, int, SmartInventoryDbContext>, ISaleRepository
    {
        public SaleRepository(SmartInventoryDbContext context) : base(context)
        {
        }

        public async Task<int> CountSalesAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<decimal> GetTotalSaleAmountAsync()
        {
            return await _dbSet.SumAsync(x => x.TotalAmount);
        }
    }
}