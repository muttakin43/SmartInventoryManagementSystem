using SmartInventory.DAL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.DAL.Interface
{
    public interface IPurchaseUnitOfWork : IUnitofWork
    {
        IPurchaseDetailsRepository PurchaseDetailsRepository { get; }
         IPurchaseRepository PurchaseRepository { get; }
        IProductRepository ProductRepository { get; }

        IStockTransactionRepository StockTransactionRepository { get; }
    }
}
