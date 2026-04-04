using SmartInventory.Contract.Request;
using SmartInventory.DAL.Migrations;
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
                CreatedTime = DateTime.Now,
                CreatedBy = 1
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


        public static Supplier MapToSupplier(this CreateSupplierRequest request)
        {
            return new Supplier
            {
                Name = request.Name,
                Phone = request.Phone,
                Address = request.Address,
                CreatedTime = DateTime.Now,
                CreatedBy = 1
            };



        }


        public static Purchase MapToPurchase(this PurchaseCreateRequest request)
        {
            return new Purchase
            {
                SupplierId = request.SupplierId,
                PurchaseDate = request.purchaseDate,
                TotalAmount = request.Details.Sum(d => d.Price * d.Quantity),
                Details = request.Details.Select(d => new PurchaseDetail
                {
                    ProductId = d.ProductId,
                    Quantity = d.Quantity,
                    Price = d.Price
                }).ToList()
            };
        }


        public static Sale MapToSale(this SaleCreateRequest request)
        {
            return new Sale
            {

                SaleDate = DateTime.UtcNow,
                SaleDetails = request.SaleDetails.Select(x => new SaleDetails
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    Price = x.Price
                }).ToList()
            };
        }
    }
}
