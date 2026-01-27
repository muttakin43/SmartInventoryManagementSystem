using Microsoft.EntityFrameworkCore;
using SmartInventory.DAL.Content;
using SmartInventory.DAL.Interface;
using SmartInventory.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.DAL.Implementation
{
    public class CategoryRepository : Repository<Category, int, SmartInventoryDbContext>,
        ICategoryRepository
    {
        public CategoryRepository(SmartInventoryDbContext context) : base(context)
        {
        }
    
public async Task<int> CountCategoriesAsync()
        {
           return await _dbSet.CountAsync();
        }
    }
}
