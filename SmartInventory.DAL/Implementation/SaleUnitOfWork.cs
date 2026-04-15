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
    public class SaleUnitOfWork : UnitofWork, ISaleUnitOfWork
    {
        public SaleUnitOfWork(
            SmartInventoryDbContext context,
            ISaleRepository saleRepository,
            IProductRepository productRepository,
            IStockTransactionRepository stockTransactionRepository,
            ICustomerRepository customerRepository

            ) : base(context)
        {
            SaleRepository = saleRepository; ;
            ProductRepository = productRepository; 
            StockTransactionRepository=stockTransactionRepository;
            CustomerRepository=customerRepository;


        }

       

        public ISaleRepository SaleRepository { get; }

      

        public IProductRepository ProductRepository { get; }

        

        public IStockTransactionRepository StockTransactionRepository { get; }

        public ICustomerRepository CustomerRepository { get; }
    }
}
