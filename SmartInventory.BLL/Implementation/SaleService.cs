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
                if (product.StockQuantit < detail.Quantity)
                    throw new Exception($"Not enough stock for {product.ProductName}");

                // 🔥 STOCK DECREASE
                product.StockQuantit -= detail.Quantity;

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
            x => x.Include(s => s.SaleDetails)
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
            x => x.Include(s => s.SaleDetails)
                  .ThenInclude(d => d.Product),
            false
        );

        var sale = data.FirstOrDefault();

        if (sale == null)
            return Result<Sale>.FailureResult("Not found");

        return Result<Sale>.SuccessResult(sale);
    }
}