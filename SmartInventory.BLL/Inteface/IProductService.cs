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
        Task<IList<Product>> GetallAsync();
        Task<Product> GetByIdAsync(int id);

        
        Task AddAsync(Product product);

        Task UpdateAsync(Product product);

        Task DeleteAsync(int id);
    }
}
