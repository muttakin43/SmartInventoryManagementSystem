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
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.BLL.Implementation
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierUnitOfWork _supplierUnitOfWork;
        public SupplierService(ISupplierUnitOfWork supplierUnitOfWork)
        {
            _supplierUnitOfWork = supplierUnitOfWork;
            
        }

        public async Task<Result<int>> AddAsync(CreateSupplierRequest supplier)
        {
            if(supplier is null)
            {
                return Result<int>.FailureResult("Supplier cannot be null");
            }
            var existSupplier = await _supplierUnitOfWork.SupplierRepository.GetAsync(
                x => x.id, x => x.Name == supplier.Name, null, null, false);
            if (existSupplier.Any())
                {
                return Result<int>.FailureResult("A supplier with the same already exist");
            }

            try
            {
                var newsupplier = supplier.MapToSupplier();
                await _supplierUnitOfWork.SupplierRepository.AddAsync(newsupplier);
                var saved = await _supplierUnitOfWork.SaveChangesAsync();
                if (!saved)
                {
                    return Result<int>.FailureResult("Failed to save supplier");
                }
                return Result<int>.SuccessResult(newsupplier.id);
            }
            catch (Exception)
            {
                return Result<int>.FailureResult("An error occurred while adding the supplier");
            }
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            var supplier = await _supplierUnitOfWork.SupplierRepository.GetByIdAsync(id);
            if (supplier is null)
            {
                return Result<bool>.FailureResult("Supplier not found");
            }

            await _supplierUnitOfWork.SupplierRepository.DeleteAsync(supplier);
            var saved = await _supplierUnitOfWork.SaveChangesAsync();
            if (saved)
                {
                return Result<bool>.SuccessResult(true);
            }
            else
            {
                return Result<bool>.FailureResult("Failed to delete supplier");
            }
        }

        public async Task<Result<IList<Supplier>>> GetallAsync()
        {
            var suppliers = await _supplierUnitOfWork.SupplierRepository.GetAsync(
                x => x, null,
                x => x.OrderByDescending(s => s.id), null, true
                );
            return Result<IList<Supplier>>.SuccessResult(suppliers.ToList());
        }

        public async Task<Result<Supplier>> GetByIdAsync(int id)
        {
            var supplier = await _supplierUnitOfWork.SupplierRepository.GetByIdAsync(id);
            if (supplier is null)
            {
                return Result<Supplier>.FailureResult("Supplier not found");
            }
            return Result<Supplier>.SuccessResult(supplier);
        }

        public async Task<Result<int>> UpdateAsync(UpdateSupplierRequest model)
        {
            var supplier = await _supplierUnitOfWork.SupplierRepository.GetByIdAsync(model.id);
            if (model is null)
            {
                return Result<int>.FailureResult("Supplier cannot be null");
            }
            var existSupplier = await _supplierUnitOfWork.SupplierRepository.GetAsync(
                x => x.id, x => x.Name == model.Name && x.id != model.id, null, null, false);
            if (existSupplier.Any())
            {
                return Result<int>.FailureResult("A supplier with the same name already exist");
            }
         
                    supplier.Name = model.Name;
                    supplier.Phone = model.Phone;
                    supplier.Address = model.Address;
                    await _supplierUnitOfWork.SupplierRepository.UpdateAsync(supplier);
                    var saved = await _supplierUnitOfWork.SaveChangesAsync();
                    if (!saved)
                    {
                        return Result<int>.FailureResult("Failed to update supplier");
                    }
                    return Result<int>.SuccessResult(supplier.id);
                }
               
            

        
        public async Task<DataTablesResponse<Supplier>> GetDataTablesAsync(DataTablesRequest request)
        {
            try
            {
                // Build search predicate
                var searchPredicate = DataTablesHelper.BuildSearchPredicate<Supplier>(
                    request,
                    searchValue =>
                    {
                        var lowerSearch = searchValue.ToLower();
                        return s =>
                            s.Name.ToLower().Contains(lowerSearch) ||
                            s.Phone.ToLower().Contains(lowerSearch) ||
                            s.Address.ToLower().Contains(lowerSearch);
                    }
                );

                // Build order by expression
                Func<IQueryable<Supplier>, IOrderedQueryable<Supplier>>? orderBy = null;

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
                                ? q => q.OrderBy(s => s.Name)
                                : q => q.OrderByDescending(s=> s.Name),
                            "phone" => isAscending
                                ? q => q.OrderBy(s =>s.Phone)
                                : q => q.OrderByDescending(s => s.Phone),
                            "address" => isAscending
                                ? q => q.OrderBy(s=> s.Address)
                                : q => q.OrderByDescending(s => s.Address),
                            
                            _ => null
                        };
                    }
                }

                // Default ordering if no order specified
                orderBy ??= q => q.OrderByDescending(s => s.id);

                // Calculate pagination
                var (pageIndex, pageSize) = DataTablesHelper.CalculatePagination(request);

                // Get Data from repository
                var (items, total, totalFilter) = await _supplierUnitOfWork.SupplierRepository.GetAsync(
                    p => p,
                    searchPredicate,
                    orderBy,
                    null,
                    pageIndex,
                    pageSize,
                    true);

                return new DataTablesResponse<Supplier>
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
