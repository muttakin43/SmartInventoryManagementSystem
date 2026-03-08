using SmartInventory.BLL.Model;
using SmartInventory.Contract.Request;
using SmartInventory.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.BLL.Inteface
{
    public interface IPurchaseService
    {
        Task<Result<IList<Purchase>>> GetallPurchaseAsync();
        Task<Result<Purchase>> GetPurchaseByIdAsync(int id);

        Task<Result<int>> CreatePurchaseAsync(PurchaseCreateRequest purchaserequest);
    }
}
