using Microsoft.EntityFrameworkCore;
using SmartInventory.DAL.Content;
using SmartInventory.DAL.Core;
using SmartInventory.DAL.Interface;
using SmartInventory.Model;

namespace SmartInventory.DAL.Implementation
{
    public class PurchaseRepository
        : Repository<Purchase, int, SmartInventoryDbContext>, IPurchaseRepository
    {
        public PurchaseRepository(SmartInventoryDbContext context) : base(context)
        {
        }

        public async Task<int> CountPurchasesAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<decimal> GetTotalPurchaseAmountAsync()
        {
            return await _dbSet.SumAsync(x => x.TotalAmount);
        }

        public async Task<IList<Purchase>> GetRecentPurchasesAsync(int count)
        {
            return await _dbSet
                .Include(x => x.Supplier)
                .OrderByDescending(x => x.PurchaseDate)
                .Take(count)
                .ToListAsync();
        }
    }
}