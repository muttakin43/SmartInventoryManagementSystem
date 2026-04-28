using Microsoft.AspNetCore.Mvc;
using SmartInventory.BLL.Inteface;
using SmartInventory.BLL.Model;
using SmartInventory.Contract.Request;
using SmartInventory.Model;

namespace SmartInventory.web.Controllers
{
    public class SaleController : Controller
    {
        private readonly ISaleService _saleService;
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;

        public SaleController(ISaleService saleService, IProductService productService, ICustomerService customerService)
        {
            _saleService = saleService;
            _productService = productService;
            _customerService = customerService;
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
            var customers = await _customerService.GetAllAsync();
            
            

            if (!products.Success || !customers.Success)
            {
                TempData["error"] = "Failed to load data";
                return RedirectToAction("Index");
            }

            ViewBag.Products = products.Data;
            ViewBag.Customers = customers.Data;
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Create(SaleCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Products = (await _productService.GetallAsync()).Data;
                ViewBag.Customers = (await _customerService.GetAllAsync()).Data;
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
            ViewBag.Customers = (await _customerService.GetAllAsync()).Data;

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

            return View(new List<Sale> { result.Data });
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
            var customers = await _customerService.GetAllAsync();
            ViewBag.Products = products.Data;
            ViewBag.Customers = customers.Data;

            var sale = result.Data;

            var request = new SaleCreateRequest
            {
                SaleDate = sale.SaleDate,
                CustomerId = sale.CustomerId,
                SaleDetails = sale.SaleDetails.Select(x => new SaleDetailsRequest
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    Price = x.Price
                }).ToList() ?? new List<SaleDetailsRequest>()
            };

            ViewBag.SaleId = id;

            return View(request);
        }


        [HttpPost]

        public async Task<IActionResult> Edit(int id, SaleCreateRequest request){

            if (!ModelState.IsValid)
            {
                ViewBag.Products = (await _productService.GetallAsync()).Data;
                ViewBag.Customers = (await _customerService.GetAllAsync()).Data;
                  
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
            ViewBag.Customers = (await _customerService.GetAllAsync()).Data;

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