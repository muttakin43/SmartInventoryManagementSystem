using Microsoft.AspNetCore.Mvc;
using SmartInventory.BLL.Inteface;
using SmartInventory.Model;

namespace SmartInventory.web.Controllers
{
    public class StockTransactionController : Controller
    {
        private readonly IStockTransactionService _service;
        private readonly IProductService _productService;

        public StockTransactionController(
            IStockTransactionService service,
            IProductService productService)
        {
            _service = service;
            _productService = productService;
        }

        public async Task<IActionResult> Index(int? productId, string type)
        {
            try
            {
                var result = await _service.GetAllAsync();

                if (!result.Success)
                {
                    TempData["error"] = result.Error;
                    return View(new List<StockTransaction>());
                }

                var data = result.Data ?? new List<StockTransaction>();

                // 🔍 FILTER: Product
                if (productId.HasValue)
                    data = data.Where(x => x.ProductId == productId.Value).ToList();

                // 🔍 FILTER: Type
                if (!string.IsNullOrWhiteSpace(type))
                    data = data.Where(x => x.Type == type).ToList();

                // 🔽 Dropdown data
                var productResult = await _productService.GetallAsync();
                ViewBag.Products = productResult.Data ?? new List<Product>();

                return View(data);
            }
            catch (Exception ex)
            {
                TempData["error"] = "Something went wrong: " + ex.Message;
                return View(new List<StockTransaction>());
            }
        }
    }
}
