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
                CategoryId = request.CategoryId,
                CreatedTime =DateTime.Now,
                CreatedBy= 1
            };
        }

        public static Category MapToCategory(this CreateCategoryRequest request)
        {
            return new Category
            {
                CategoryName = request.CategoryName,
                Description = request.Description,
                CreatedTime = DateTime.Now,
                CreatedBy = 1
            };
        }



    }
}
