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
using System.Threading.Tasks;

namespace SmartInventory.BLL.Implementation
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseUnitOfWork _purchaseUnitOfWork;

        public PurchaseService(IPurchaseUnitOfWork purchaseUnitOfWork)
        {
            _purchaseUnitOfWork = purchaseUnitOfWork;
        }

        // CREATE PURCHASE
        public async Task<Result<int>> CreatePurchaseAsync(PurchaseCreateRequest purchaseRequest)
        {
            try
            {
                var purchase = purchaseRequest.MapToPurchase();

                foreach (var detail in purchase.Details)
                {
                    var product = await _purchaseUnitOfWork.ProductRepository.GetByIdAsync(detail.ProductId);

                    if (product == null)
                        throw new Exception("Product not found");

                    // ✅ Increase stock
                    product.StockQuantity += detail.Quantity;

                    // ✅ Add stock transaction
                    await _purchaseUnitOfWork.StockTransactionRepository.AddAsync(new StockTransaction
                    {
                        ProductId = detail.ProductId,
                        Quantity = detail.Quantity,
                        Type = "Purchase",
                        Date = DateTime.UtcNow
                    });
                }

                // ✅ Calculate total amount
                purchase.TotalAmount = purchase.Details.Sum(x => x.Quantity * x.Price);

                await _purchaseUnitOfWork.PurchaseRepository.AddAsync(purchase);
                await _purchaseUnitOfWork.SaveChangesAsync();

                return Result<int>.SuccessResult(purchase.id);
            }
            catch (Exception ex)
            {
                return Result<int>.FailureResult($"Purchase creation failed: {ex.Message}");
            }
        }

        // GET ALL PURCHASES
        public async Task<Result<IList<Purchase>>> GetallPurchaseAsync()
        {
            try
            {
                var purchases = await _purchaseUnitOfWork.PurchaseRepository.GetAsync<Purchase>(
                    selector: x => x,
                    predicate: null,
                    orderBy: null,
                    include: x => x.Include(p => p.Supplier)
                                  .Include(p => p.Details)
                                  .ThenInclude(d => d.Product),
                    disableTracking:
                    true
                );

                return Result<IList<Purchase>>.SuccessResult(purchases);
            }
            catch (Exception ex)
            {
                return Result<IList<Purchase>>.FailureResult($"Failed to fetch purchases: {ex.Message}");
            }
        }

        // GET PURCHASE BY ID (WITH Supplier & Details & Product)
        public async Task<Result<Purchase>> GetPurchaseByIdAsync(int id)
        {
            try
            {
                var purchases = await _purchaseUnitOfWork.PurchaseRepository.GetAsync<Purchase>(
                    selector: x => x,
                    predicate: x => x.id == id,
                    orderBy: null,
                    include: x => x.Include(p => p.Supplier)
                                  .Include(p => p.Details)
                                  .ThenInclude(d => d.Product),
                    disableTracking: false
                );

                var purchase = purchases.FirstOrDefault();

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