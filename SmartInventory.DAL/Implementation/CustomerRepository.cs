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
    public class CustomerRepository : Repository<Customer, int, SmartInventoryDbContext>,
        ICustomerRepository
    {
        public CustomerRepository(Content.SmartInventoryDbContext context) : base(context)
        {
        }

        public async Task<Customer> GetCustomerWithSales(int id)
        {
            return await _context.Customers
           .Include(c => c.Sales)
           .FirstOrDefaultAsync(c => c.id == id);
        }
    }
}
