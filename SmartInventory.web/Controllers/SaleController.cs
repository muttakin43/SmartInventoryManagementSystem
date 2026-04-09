using Microsoft.AspNetCore.Mvc;
using SmartInventory.BLL.Inteface;
using SmartInventory.BLL.Model;
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

        public async Task<IActionResult> Edit(int id)
        {
            var result= await _saleService.GetByIdAsync(id);
            if (!result.Success)
            {
                TempData["error"] = result.Error;
                return RedirectToAction("Index");
            }

            var products= await _productService.GetallAsync();
            ViewBag.Product = products.Data;

            var sale = result.Data;

            var request = new SaleCreateRequest
            {
                SaleDate = sale.SaleDate,
                SaleDetails = sale.SaleDetails.Select(x => new SaleDetailsRequest
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    Price = x.Price
                }).ToList()
            };

            ViewBag.SaleId = id;

            return View(request);
        }


        [HttpPost]

        public async Task<IActionResult> Edit(int id, SaleCreateRequest request){

            if (!ModelState.IsValid)
            {
                ViewBag.Products = (await _productService.GetallAsync()).Data;
                return View(request);
            }

            var result = await _saleService.UpdateSaleAsync(id, request);

            if (result.Success)
            {
                TempData["success"] = "Sale updated successfully";
                return RedirectToAction("Index");
            }

            TempData["error"] = result.Error;
            ViewBag.Products = (await _productService.GetallAsync()).Data;

            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _saleService.DeleteSaleAsync(id);

            if (result.Success)
            {
                TempData["success"] = "Sale deleted successfully";
            }
            else
            {
                TempData["error"] = result.Error;
            }

            return RedirectToAction("Index");
        }


    }
} 