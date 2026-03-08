using Microsoft.EntityFrameworkCore;
using SmartInventory.BLL.Inteface;
using SmartInventory.BLL.Mapping;
using SmartInventory.BLL.Model;
using SmartInventory.Contract.Request;
using SmartInventory.DAL.Interface;
using SmartInventory.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.BLL.Implementation
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseUnitOfWork _purchaseUnitofWork;
        public PurchaseService(IPurchaseUnitOfWork purchaseUnitOfWork)
        {
            _purchaseUnitofWork = purchaseUnitOfWork;
        }

        public async Task<Result<int>> CreatePurchaseAsync(PurchaseCreateRequest purchaserequest)
        {
            try
            {
                var purchase = purchaserequest.MapToPurchase();

                foreach (var detail in purchase.Details)
                {
                    // Make sure GetAsync exists in IProductRepository
                    var product = await _purchaseUnitofWork.ProductRepository.GetByIdAsync(detail.ProductId);
                    if (product != null)
                        product.StockQuantit += detail.Quantity;
                }

                await _purchaseUnitofWork.PurchaseRepository.AddAsync(purchase);
                await _purchaseUnitofWork.SaveChangesAsync();

                return Result<int>.SuccessResult(purchase.id);
            }
            catch (Exception ex)
            {
                return Result<int>.FailureResult($"Purchase creation failed: {ex.Message}");
            }
        }



        public async Task<Result<IList<Purchase>>> GetallPurchaseAsync()
        {
            try
            {
                var purchases = await _purchaseUnitofWork.PurchaseRepository.GetAsync<Purchase>(
                    selector: x => x,
                    predicate: null,
                    orderBy: null,
                    include: null,
                    disableTracking: true
                );

                return Result<IList<Purchase>>.SuccessResult(purchases);
            }
            catch (Exception ex)
            {
                return Result<IList<Purchase>>.FailureResult($"Failed to fetch purchases: {ex.Message}");
            }
        }

        public async Task<Result<Purchase>> GetPurchaseByIdAsync(int id)
        {
            try
            {
                var purchase = await _purchaseUnitofWork.PurchaseRepository.GetByIdAsync(id);

                if (purchase == null)
                    return Result<Purchase>.FailureResult("Purchase not found");

                return Result<Purchase>.SuccessResult(purchase);
            }
            catch (Exception ex)
            {
                return Result<Purchase>.FailureResult($"Failed to fetch purchase: {ex.Message}");
            }
        }
    }
}
