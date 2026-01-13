using SmartInventory.BLL.Inteface;
using SmartInventory.BLL.Mapping;
using SmartInventory.BLL.Model;
using SmartInventory.Contract.Request;
using SmartInventory.DAL.Implementation;
using SmartInventory.DAL.Interface;
using SmartInventory.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace SmartInventory.BLL.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductUnitofWork _productUnitofWork;

        public ProductService(IProductUnitofWork productUnitofWork)
        {
            _productUnitofWork = productUnitofWork;
        }


        public async Task<Result<int>> AddAsync(CreateProductRequest product)
        {
            if (product is null)
            {
                return Result<int>.FailureResult("Product cannot be null");

            }
            var existProduct = await _productUnitofWork.ProductRepository.GetAsync(
                x => x.id, x => x.Name == product.Name, null, null, false);
            if (existProduct.Any())
            {
                return Result<int>.FailureResult("A product with the same already exist");
            }



            try
            {
                var newproduct = product.MapToProduct();
                await _productUnitofWork.ProductRepository.AddAsync(newproduct);
                var saved=await _productUnitofWork.SaveChangesAsync();

                if (!saved)
                {
                    return Result<int>.FailureResult("Failed to save product");
                }
                return Result<int>.SuccessResult(newproduct.id);
            }
            catch (Exception )
            {
                return Result<int>.FailureResult("An error occurred while adding the product");
            }
        }
     

        public async Task<Result<bool>> DeleteAsync(int id)
        {
           var product=await _productUnitofWork.ProductRepository.GetByIdAsync(id);

            if(product is null)
            {
                return Result<bool>.FailureResult("Product not found");
            }
            await _productUnitofWork.ProductRepository.DeleteAsync(product);
            var saved= await _productUnitofWork.SaveChangesAsync();
           if(!saved)
            {
                return Result<bool>.FailureResult("Failed to delete product");
            }
            return Result<bool>.SuccessResult(true);
        }

        public async Task<Result<IList<Product>>> GetallAsync()
        {
           var products=await _productUnitofWork.ProductRepository.GetAsync(
               x=> x, null, 
               x=>x.OrderByDescending(x=>x.id),null,true);
            return Result<IList<Product>>.SuccessResult(products);
        }

        public async Task<Result<Product>> GetByIdAsync(int id)
        {
            var product = await _productUnitofWork.ProductRepository.GetByIdAsync(id);
          if(product is null)
            {
                return Result<Product>.FailureResult($"Product with id{id} is not found");
            }
            return Result<Product>.SuccessResult(product);
        }

        public async Task<Result<int>> UpdateAsync(UpdateProductRequest model)
        {
            if (model is null)
            {
                return Result<int>.FailureResult("Product data cannot be null.");
            }

            var product = await _productUnitofWork.ProductRepository.GetByIdAsync(model.id);
            if (product is null)
            {
                return Result<int>.FailureResult($"Product with id {model.id} was not found.");
            }
       

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.StockQuantit = model.StockQuantit;

            await _productUnitofWork.ProductRepository.UpdateAsync(product);

            var saved = await _productUnitofWork.SaveChangesAsync();

            if (!saved)
            {
                return Result<int>.FailureResult("Failed to update product.");
            }

            return Result<int>.SuccessResult(model.id);
        }


    }
}
