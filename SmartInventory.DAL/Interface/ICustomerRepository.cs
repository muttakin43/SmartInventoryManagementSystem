using SmartInventory.DAL.Content;
using SmartInventory.DAL.Core;
using SmartInventory.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.DAL.Interface
{
    public interface ICustomerRepository : IRepository<Customer, int, SmartInventoryDbContext>
    {
        Task<Customer> GetCustomerWithSales(int id);
    }
}
