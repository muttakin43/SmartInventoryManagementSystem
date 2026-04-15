using SmartInventory.DAL.Content;
using SmartInventory.DAL.Core;
using SmartInventory.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.DAL.Interface
{
    public interface IPurchaseRepository : IRepository<Purchase,int,SmartInventoryDbContext>
    {
        Task<int> CountPurchasesAsync();

        Task<decimal> GetTotalPurchaseAmountAsync();

        Task<IList<Purchase>> GetRecentPurchasesAsync(int count);
        void Remove(Purchase purchase);
    }
}
