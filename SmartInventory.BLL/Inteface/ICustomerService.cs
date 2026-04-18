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
    public interface ICustomerService
    {
        Task<Result<int>> CreateAsync(CustomerCreateRequest request);
        Task<Result<bool>> UpdateAsync(int id, CustomerCreateRequest request);
        Task<Result<IList<CustomerResponse>>> GetAllAsync();
        Task<Result<CustomerResponse>> GetByIdAsync(int id);
        Task<Result<bool>> DeleteAsync(int id);
    }
}
