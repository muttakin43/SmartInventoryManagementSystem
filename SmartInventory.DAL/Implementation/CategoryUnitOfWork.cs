using SmartInventory.DAL.Content;
using SmartInventory.DAL.Core;
using SmartInventory.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.DAL.Implementation
{
    public class CategoryUnitOfWork : UnitofWork, ICategoryUnitOfWork
    {
        public CategoryUnitOfWork(
            SmartInventoryDbContext context,
            ICategoryRepository categoryRepository) : base(context)
        {
            CategoryRepository = categoryRepository;
        }

        public ICategoryRepository CategoryRepository { get; }
    }
}
