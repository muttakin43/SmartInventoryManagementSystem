using Microsoft.EntityFrameworkCore;
using SmartInventory.BLL.Inteface;
using SmartInventory.BLL.Mapping;
using SmartInventory.BLL.Model;
using SmartInventory.Contract.Request;
using SmartInventory.DAL.Interface;
using SmartInventory.Model;

public class SaleService : ISaleService
{
    private readonly ISaleUnitOfWork _saleUnitOfWork;

    public SaleService(ISaleUnitOfWork saleUnitOfWork)
    {
        _saleUnitOfWork = saleUnitOfWork;
    }

    public async Task<Result<int>> CreateSaleAsync(SaleCreateRequest request)
    {
        try
        {
            var sale = request.MapToSale();

            foreach (var detail in sale.SaleDetails)
            {
                var product = await _saleUnitOfWork.ProductRepository.GetByIdAsync(detail.ProductId);

                if (product == null)
                    throw new Exception("Product not found");

                // ❗ STOCK CHECK
                if (product.StockQuantity < detail.Quantity)
                    throw new Exception($"Not enough stock for {product.ProductName}");

                // 🔥 STOCK DECREASE
                product.StockQuantity -= detail.Quantity;

                // 🔥 STOCK TRANSACTION
                await _saleUnitOfWork.StockTransactionRepository.AddAsync(new StockTransaction
                {
                    ProductId = detail.ProductId,
                    Quantity = detail.Quantity,
                    Type = "Sale",
                    Date = DateTime.UtcNow
                });
            }

            // 🔥 TOTAL
            sale.TotalAmount = sale.SaleDetails.Sum(x => x.Quantity * x.Price);

            await _saleUnitOfWork.SaleRepository.AddAsync(sale);
            await _saleUnitOfWork.SaveChangesAsync();

            return Result<int>.SuccessResult(sale.id);
        }
        catch (Exception ex)
        {
            return Result<int>.FailureResult(ex.Message);
        }
    }

    

    public async Task<Result<IList<Sale>>> GetAllAsync()
    {
        var data = await _saleUnitOfWork.SaleRepository.GetAsync<Sale>(
            x => x,
            null,
            null,
            x => x
            .Include(s=> s.Customer)
            .Include(s => s.SaleDetails)
                  .ThenInclude(d => d.Product),
            true
        );

        return Result<IList<Sale>>.SuccessResult(data);
    }

    public async Task<Result<Sale>> GetByIdAsync(int id)
    {
        var data = await _saleUnitOfWork.SaleRepository.GetAsync<Sale>(
            x => x,
            x => x.id == id,
            null,
            x => x
             .Include(s => s.Customer)
            .Include(s => s.SaleDetails)
                  .ThenInclude(d => d.Product),
            false
        );

        var sale = data.FirstOrDefault();

        if (sale == null)
            return Result<Sale>.FailureResult("Not found");

        return Result<Sale>.SuccessResult(sale);
    }
    public async Task<Result<bool>> UpdateSaleAsync(int id, SaleCreateRequest request)
    {
        try
        {
            var existingSale = await _saleUnitOfWork.SaleRepository
                .GetAsync<Sale>(
                    x => x,
                    x => x.id == id,
                    null,
                    x => x.Include(s => s.SaleDetails),
                    false
                );

            var sale = existingSale.FirstOrDefault();

            if (sale == null)
                return Result<bool>.FailureResult("Sale not found");

            if (request.SaleDetails == null || !request.SaleDetails.Any())
                return Result<bool>.FailureResult("No sale items provided");

            // 🔥 Update SaleDate
            sale.SaleDate = request.SaleDate;

            // 🔁 OLD STOCK BACK
            foreach (var old in sale.SaleDetails)
            {
                var product = await _saleUnitOfWork.ProductRepository.GetByIdAsync(old.ProductId);

                if (product != null)
                {
                    product.StockQuantity += old.Quantity;
                }
            }

            // 🧹 CLEAR OLD DETAILS
            sale.SaleDetails.Clear();


            // 🔥 ADD NEW DETAILS
            foreach (var detail in request.SaleDetails)
            {
                var product = await _saleUnitOfWork.ProductRepository.GetByIdAsync(detail.ProductId);

                if (product == null)
                    throw new Exception("Product not found");

                if (product.StockQuantity < detail.Quantity)
                    throw new Exception($"Not enough stock for {product.ProductName}");

                product.StockQuantity -= detail.Quantity;

                sale.SaleDetails.Add(new SaleDetails
                {
                    ProductId = detail.ProductId,
                    Quantity = detail.Quantity,
                    Price = detail.Price
                });

               
                await _saleUnitOfWork.StockTransactionRepository.AddAsync(new StockTransaction
                {
                    ProductId = detail.ProductId,
                    Quantity = detail.Quantity,
                    Type = "Sale Update",
                    Date = DateTime.UtcNow
                });
            }

           
            sale.TotalAmount = sale.SaleDetails.Sum(x => x.Quantity * x.Price);

            await _saleUnitOfWork.SaveChangesAsync();

            return Result<bool>.SuccessResult(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.FailureResult(ex.Message);
        }
    }
    public async Task<Result<bool>> DeleteSaleAsync(int id)
    {
        try
        {
            // 1️⃣ Get existing sale with details
            var existingSale = await _saleUnitOfWork.SaleRepository
                .GetAsync<Sale>(
                    x => x,
                    x => x.id == id,
                    null,
                    x => x.Include(s => s.SaleDetails),
                    false
                );

            var sale = existingSale.FirstOrDefault();

            if (sale == null)
                return Result<bool>.FailureResult("Sale not found");

            // 2️⃣ Rollback stock for all products
            foreach (var detail in sale.SaleDetails)
            {
                var product = await _saleUnitOfWork.ProductRepository.GetByIdAsync(detail.ProductId);
                if (product != null)
                {
                    product.StockQuantity += detail.Quantity;

                    // 🔥 Optional: Log stock rollback transaction
                    await _saleUnitOfWork.StockTransactionRepository.AddAsync(new StockTransaction
                    {
                        ProductId = detail.ProductId,
                        Quantity = detail.Quantity,
                        Type = "Sale Deleted",
                        Date = DateTime.UtcNow
                    });
                }
            }

            // 3️⃣ Delete sale
            _saleUnitOfWork.SaleRepository.Remove(sale);
            await _saleUnitOfWork.SaveChangesAsync();

            return Result<bool>.SuccessResult(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.FailureResult(ex.Message);
        }
    }


}