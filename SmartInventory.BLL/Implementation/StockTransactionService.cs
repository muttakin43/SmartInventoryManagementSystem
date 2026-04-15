using Microsoft.EntityFrameworkCore;
using SmartInventory.BLL.Inteface;
using SmartInventory.BLL.Model;
using SmartInventory.DAL.Interface;
using SmartInventory.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.BLL.Implementation
{
    public class StockTransactionService : IStockTransactionService
    {
        private readonly IPurchaseUnitOfWork _unitOfWork;

        public StockTransactionService(IPurchaseUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public async Task<Result<IList<StockTransaction>>> GetAllAsync()
        {
            try
            {
                var data = await _unitOfWork.StockTransactionRepository.GetAsync<StockTransaction>(
       
                    x => x,
                    null,
                    x => x.OrderByDescending(x => x.Date),
                    x => x.Include(t => t.Product), 
                    true
                );

                return Result<IList<StockTransaction>>.SuccessResult(data);
            }
            catch (Exception ex)
            {
                return Result<IList<StockTransaction>>.FailureResult(ex.Message);
            }
        }
    }
}
