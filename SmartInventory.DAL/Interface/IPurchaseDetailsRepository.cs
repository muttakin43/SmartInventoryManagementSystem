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
    public interface IPurchaseDetailsRepository : IRepository<PurchaseDetail,int,SmartInventoryDbContext>
    {
        Task<int> CountPurchaseItemsAsync();

        Task<int> GetTotalPurchasedQuantityAsync(int productId);
    }
}
