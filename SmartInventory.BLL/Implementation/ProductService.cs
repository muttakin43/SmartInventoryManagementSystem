using SmartInventory.BLL.Helpers;
using SmartInventory.BLL.Inteface;
using SmartInventory.BLL.Mapping;
using SmartInventory.BLL.Model;
using SmartInventory.Contract.Request;
using SmartInventory.Contract.Response;
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
                x => x.id, x => x.ProductName == product.Name, null, null, false);
            if (existProduct.Any())
            {
                return Result<int>.FailureResult("A product with the same already exist");
            }



            try
            {
                var newproduct = product.MapToProduct();
                await _productUnitofWork.ProductRepository.AddAsync(newproduct);
                var saved = await _productUnitofWork.SaveChangesAsync();

                if (!saved)
                {
                    return Result<int>.FailureResult("Failed to save product");
                }
                return Result<int>.SuccessResult(newproduct.id);
            }
            catch (Exception)
            {
                return Result<int>.FailureResult("An error occurred while adding the product");
            }
        }


        public async Task<Result<bool>> DeleteAsync(int id)
        {
            var product = await _productUnitofWork.ProductRepository.GetByIdAsync(id);

            if (product is null)
            {
                return Result<bool>.FailureResult("Product not found");
            }
            await _productUnitofWork.ProductRepository.DeleteAsync(product);
            var saved = await _productUnitofWork.SaveChangesAsync();
            if (!saved)
            {
                return Result<bool>.FailureResult("Failed to delete product");
            }
            return Result<bool>.SuccessResult(true);
        }

        public async Task<Result<IList<Product>>> GetallAsync()
        {
            var products = await _productUnitofWork.ProductRepository.GetAsync(
                x => x, null,
                x => x.OrderByDescending(x => x.id), null, true);
            return Result<IList<Product>>.SuccessResult(products);
        }

        public async Task<Result<Product>> GetByIdAsync(int id)
        {
            var product = await _productUnitofWork.ProductRepository.GetByIdAsync(id);
            if (product is null)
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
            // Check if another product with the same name already exists
            var existProduct = await _productUnitofWork.ProductRepository.GetAsync(
                x => x.id,
                x => x.ProductName == model.Name && x.id != model.id, // exclude current product
                null,
                null,
                false
            );

            if (existProduct.Any())
            {
                return Result<int>.FailureResult("A product with the same name already exists.");
            }


            product.ProductName = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.StockQuantit = model.StockQuantit;
            product.CategoryId = model.CategoryId;

            await _productUnitofWork.ProductRepository.UpdateAsync(product);

            var saved = await _productUnitofWork.SaveChangesAsync();

            if (!saved)
            {
                return Result<int>.FailureResult("Failed to update product.");
            }

            return Result<int>.SuccessResult(model.id);
        }


        public async Task<DataTablesResponse<Product>> GetDataTablesAsync(DataTablesRequest request)
        {
            try
            {
                // Build search predicate
                var searchPredicate = DataTablesHelper.BuildSearchPredicate<Product>(
                    request,
                    searchValue =>
                    {
                        var lowerSearch = searchValue.ToLower();
                        return p =>
                            p.ProductName.ToLower().Contains(lowerSearch) ||
                            p.Description.ToLower().Contains(lowerSearch) ||
                            p.Price.ToString().Contains(searchValue) ||
                            p.StockQuantit.ToString().Contains(searchValue);
                    }
                );

                // Build order by expression
                Func<IQueryable<Product>, IOrderedQueryable<Product>>? orderBy = null;

                if (request.Order != null && request.Order.Any() && request.Columns != null)
                {
                    var order = request.Order.First();
                    var columnIndex = order.Column;
                    var isAscending = order.Dir.ToLower() == "asc";

                    if (columnIndex >= 0 && columnIndex < request.Columns.Count)
                    {
                        var column = request.Columns[columnIndex];
                        var columnKey = column.Data.ToLower();

                        orderBy = columnKey switch
                        {
                            "name" => isAscending
                                ? q => q.OrderBy(p => p.ProductName)
                                : q => q.OrderByDescending(p => p.ProductName),
                            "description" => isAscending
                                ? q => q.OrderBy(p => p.Description)
                                : q => q.OrderByDescending(p => p.Description),
                            "price" => isAscending
                                ? q => q.OrderBy(p => p.Price)
                                : q => q.OrderByDescending(p => p.Price),
                            "stockquantit" => isAscending
                                ? q => q.OrderBy(p => p.StockQuantit)
                                : q => q.OrderByDescending(p => p.StockQuantit),
                            _ => null
                        };
                    }
                }

                // Default ordering if no order specified
                orderBy ??= q => q.OrderByDescending(p => p.id);

                // Calculate pagination
                var (pageIndex, pageSize) = DataTablesHelper.CalculatePagination(request);

                // Get Data from repository
                var (items, total, totalFilter) = await _productUnitofWork.ProductRepository.GetAsync(
                    p => p,
                    searchPredicate,
                    orderBy,
                    null,
                    pageIndex,
                    pageSize,
                    true);

                return new DataTablesResponse<Product>
                {
                    Draw = request.Draw,
                    RecordsTotal = total,
                    RecordsFiltered = totalFilter,
                    Data = items.ToList()
                };

            }
            catch (Exception)
            {

                throw;
            }


        }
    }
}
