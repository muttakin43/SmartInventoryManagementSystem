using SmartInventory.BLL.Model;
using SmartInventory.Contract.Request;
using SmartInventory.Contract.Response;
using SmartInventory.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.BLL.Inteface
{
    public interface ISupplierService
    {
        Task<Result<IList<Supplier>>> GetallAsync();

        Task<Result<Supplier>> GetByIdAsync(int id);

        Task<Result<int>> AddAsync(CreateSupplierRequest supplier);
        Task<Result<int>> UpdateAsync(UpdateSupplierRequest supplier);

        Task<Result<bool>> DeleteAsync(int id);
        Task<DataTablesResponse<Supplier>> GetDataTablesAsync(DataTablesRequest request);
    }
}
