using SmartInventory.BLL.Model;
using SmartInventory.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.BLL.Inteface
{
    public interface IStockTransactionService
    {
        Task<Result<IList<StockTransaction>>> GetAllAsync();
    }
}
