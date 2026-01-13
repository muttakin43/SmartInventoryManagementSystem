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
    public interface IProductService
    {
        Task<Result<IList<Product>>> GetallAsync();
        Task<Result<Product>> GetByIdAsync(int id);


        Task<Result<int>> AddAsync(CreateProductRequest product);

        Task<Result<int>> UpdateAsync(UpdateProductRequest product);

        Task<Result<bool>> DeleteAsync(int id);

        Task<DataTablesResponse<Product>> GetDataTablesAsync(DataTablesRequest request);
    }
}
