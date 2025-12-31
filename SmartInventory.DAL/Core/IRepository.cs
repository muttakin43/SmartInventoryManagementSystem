using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.DAL.Core
{
    public interface IRepository<TEntity,TKey,TContext>
        where TEntity : class
        where TContext : DbContext
    {
        Task AddAsyc(TEntity entity);
    }
}
