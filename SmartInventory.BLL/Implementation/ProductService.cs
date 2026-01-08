using SmartInventory.BLL.Inteface;
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
        public async Task AddAsync(Product product)
        {
           
                await _productUnitofWork.ProductRepository.AddAsync(product);
                await _productUnitofWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
           var product=await _productUnitofWork.ProductRepository.GetByIdAsync(id);
            if(product is not null)
            {
                await _productUnitofWork.ProductRepository.DeleteAsync(product);
            }
        }

        public Task<IList<Product>> GetallAsync()
        {
           var products= _productUnitofWork.ProductRepository.GetAsync(x=> x, null, x=>x.OrderByDescending(x=>x.id),null,true);
            return products;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            var product = await _productUnitofWork.ProductRepository.GetByIdAsync(id);
            return product;
        }

        public async Task UpdateAsync(Product product)
        {
            var existproduct = await _productUnitofWork.ProductRepository.GetByIdAsync(product.id); 

            if (existproduct is not null)
            {
                existproduct.Name= product.Name;

                await _productUnitofWork.ProductRepository.UpdateAsync(product);
                await _productUnitofWork.SaveChangesAsync();
            }

        }
    }
}
