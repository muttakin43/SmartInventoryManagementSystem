using SmartInventory.DAL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.DAL.Interface
{
    public interface IProductUnitofWork : IUnitofWork
    {
        IProductRepository ProductRepository { get; }
    }
}
