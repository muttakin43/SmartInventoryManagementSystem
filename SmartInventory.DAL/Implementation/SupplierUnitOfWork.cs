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
    public class SupplierUnitOfWork : UnitofWork, ISupplierUnitOfWork
    {
        public SupplierUnitOfWork(SmartInventoryDbContext context,
            ISupplierRepository supplierRepository) : base(context)
        {
            SupplierRepository = supplierRepository;
        }
        public ISupplierRepository SupplierRepository { get; }
    }
}
