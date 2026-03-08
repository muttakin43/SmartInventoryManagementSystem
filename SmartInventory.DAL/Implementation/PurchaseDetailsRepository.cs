using Microsoft.EntityFrameworkCore;
using SmartInventory.DAL.Content;
using SmartInventory.DAL.Core;
using SmartInventory.DAL.Interface;
using SmartInventory.Model;

namespace SmartInventory.DAL.Implementation
{
    public class PurchaseDetailsRepository
        : Repository<PurchaseDetail, int, SmartInventoryDbContext>, IPurchaseDetailsRepository
    {
        public PurchaseDetailsRepository(SmartInventoryDbContext context) : base(context)
        {
        }

        public async Task<int> CountPurchaseItemsAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<int> GetTotalPurchasedQuantityAsync(int productId)
        {
            return await _dbSet
                .Where(x => x.ProductId == productId)
                .SumAsync(x => x.Quantity);
        }
    }
}