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
    public interface ICategoryService
    {
        Task<Result<IList<Category>>> GetallAsync();
        Task<Result<Category>> GetByIdAsync(int id);

        Task<Result<int>> AddAsync(CreateCategoryRequest category);

        Task<Result<int>> UpdateAsync(UpdateCategoryRequest category);

        Task<Result<bool>> DeleteAsync(int id);



    }
}
