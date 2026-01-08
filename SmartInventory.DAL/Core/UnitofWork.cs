using Microsoft.EntityFrameworkCore;
using SmartInventory.DAL.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.DAL.Core
{
    public class UnitofWork : IUnitofWork
    {
        private bool _disposed;
        private readonly SmartInventoryDbContext _context;


        public UnitofWork(SmartInventoryDbContext context)
        {
            _context = context;
        }


        public bool SaveChanges()
        {
            return _context?.SaveChanges() > 0;
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
       

        public void Rollback()
        {
            _context.ChangeTracker.Entries()
                .ToList()
                .ForEach(x=>x.Reload());
        }
        #region Dispose

        ~UnitofWork()
        {
            Dispose(false);
        }
        public void Dispose()
        {
                       Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context?.Dispose();
                }
                _disposed = true;
            }
            #endregion
        }



    }
}
