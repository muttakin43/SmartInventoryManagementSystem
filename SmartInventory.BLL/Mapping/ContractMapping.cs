using SmartInventory.Contract.Request;
using SmartInventory.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.BLL.Mapping
{
    public static class ContractMapping
    {
        public static Product MapToProduct(this CreateProductRequest request)
        {
          
            return new Product
            {
                
                ProductName = request.Name,
                Description = request.Description,
                Price = request.Price,
                StockQuantit = request.StockQuantit,
                CreatedTime=DateTime.Now,
                CreatedBy= 1
            };
        }


      
    }
}
