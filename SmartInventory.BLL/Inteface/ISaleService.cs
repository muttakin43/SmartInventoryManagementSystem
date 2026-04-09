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
    public interface ISaleService
    {
        Task<Result<int>> CreateSaleAsync(SaleCreateRequest request);
        Task<Result<IList<Sale>>> GetAllAsync();
        Task<Result<Sale>> GetByIdAsync(int id);
        Task<Result<bool>> UpdateSaleAsync(int id, SaleCreateRequest request);
        Task<Result<bool>> DeleteSaleAsync(int id);

    }
}
