using Microsoft.AspNetCore.Mvc;
using SmartInventory.BLL.Inteface;
using SmartInventory.Contract.Request;

namespace SmartInventory.web.Controllers
{
    public class SaleController : Controller
    {
        private readonly ISaleService _saleService;
        private readonly IProductService _productService;

        public SaleController(ISaleService saleService, IProductService productService)
        {
            _saleService = saleService;
            _productService = productService;
        }

        // 📋 LIST
        public async Task<IActionResult> Index()
        {
            var result = await _saleService.GetAllAsync();

            if (!result.Success)
            {
                TempData["error"] = result.Error;
                return View(new List<object>());
            }

            return View(result.Data);
        }

        // 🟢 CREATE (GET)
        public async Task<IActionResult> Create()
        {
            var products = await _productService.GetallAsync();

            if (!products.Success)
            {
                TempData["error"] = "Failed to load products";
                return RedirectToAction("Index");
            }

            ViewBag.Products = products.Data;
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Create(SaleCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Products = (await _productService.GetallAsync()).Data;
                return View(request);
            }

            var result = await _saleService.CreateSaleAsync(request);

            if (result.Success)
            {
                TempData["success"] = "Sale created successfully";
                return RedirectToAction("Index");
            }

            TempData["error"] = result.Error;
            ViewBag.Products = (await _productService.GetallAsync()).Data;

            return View(request);
        }

     
        public async Task<IActionResult> Details(int id)
        {
            var result = await _saleService.GetByIdAsync(id);

            if (!result.Success)
            {
                TempData["error"] = result.Error;
                return RedirectToAction("Index");
            }

            return View(result.Data);
        }
    }
}