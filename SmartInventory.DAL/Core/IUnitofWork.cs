using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.DAL.Core
{
    public interface IUnitofWork : IDisposable
    {
        bool SaveChanges();
        void Rollback();
        Task<bool> SaveChangesAsync();
    }
}
