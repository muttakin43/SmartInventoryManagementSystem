using SmartInventory.BLL.Model;
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

        Task<Result<int>> AddAsync(Category category);

        Task<Result<int>> UpdateAsync(Category category);

        Task<Result<bool>> DeleteAsync(int id);



    }
}
