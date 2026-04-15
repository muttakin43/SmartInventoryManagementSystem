using Microsoft.AspNetCore.Mvc;
using SmartInventory.BLL.Implementation;
using SmartInventory.BLL.Inteface;
using SmartInventory.BLL.Model;
using SmartInventory.Contract.Request;
using SmartInventory.Model;

namespace SmartInventory.web.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly IPurchaseService _purchaseService;
        private readonly IProductService _productService;
        private readonly ISupplierService _supplierService;
        public PurchaseController(IPurchaseService purchaseService, IProductService productService, ISupplierService supplierService)
        {
            _purchaseService = purchaseService;
            _productService = productService;
            _supplierService = supplierService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _purchaseService.GetallPurchaseAsync();
            if (!result.Success)
            {

                TempData["Error"] = result.Error;
                return View(new List<object>());

            }
            return View(result.Data);
        }

        public async Task<IActionResult> Create()
        {
            var products = await _productService.GetallAsync();
            var suppliers = await _supplierService.GetallAsync();

            if (!products.Success || !suppliers.Success)
            {
                TempData["error"] = "Failed to load products or suppliers";
                return RedirectToAction("Index");
            }

            ViewBag.Products = products.Data;
            ViewBag.Suppliers = suppliers.Data;

            return View();
        }

        // Save Purchase

        [HttpPost]
        public async Task<IActionResult> Create(PurchaseCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                // ✅ Add this to reload products and suppliers
                ViewBag.Products = (await _productService.GetallAsync()).Data;
                ViewBag.Suppliers = (await _supplierService.GetallAsync()).Data;

                return View(request);
            }

            var result = await _purchaseService.CreatePurchaseAsync(request);

            if (result.Success)
            {
                TempData["success"] = "Purchase created successfully";
                return RedirectToAction("Index");
            }

            TempData["error"] = result.Error;

            // If something fails, reload Products & Suppliers again
            ViewBag.Products = (await _productService.GetallAsync()).Data;
            ViewBag.Suppliers = (await _supplierService.GetallAsync()).Data;

            return View(request);
        }


        public async Task<IActionResult> Details(int id)
        {
            var result = await _purchaseService.GetPurchaseByIdAsync(id);

            if (!result.Success)
            {
                TempData["error"] = result.Error;
                return RedirectToAction("Index");
            }

            return View(result.Data);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var purchase = await _purchaseService.GetPurchaseByIdAsync(id);
            if (!purchase.Success)
            {
                TempData["error"] = purchase.Error;
                return RedirectToAction("Index");
            }

            var res = purchase.Data;

            var model = new PurchaseUpdateRequest
            {
                SupplierId = res.SupplierId,
                Items = res.Details.Select(x => new PurchaseDetailsUpdateRequest
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    UnitPrice = x.Price
                }).ToList()
            };

            ViewBag.Products = _productService.GetallAsync().Result.Data;
            ViewBag.Suppliers = _supplierService.GetallAsync().Result.Data;

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, PurchaseUpdateRequest request)
        {
            var result = await _purchaseService.UpdatePurchaseAsync(id, request);

            if (result.Success)
            {
                TempData["success"] = "Purchase updated successfully!";
                return RedirectToAction("Index");
            }

            TempData["error"] = result.Error;

            ViewBag.Products = _productService.GetallAsync().Result.Data;
            ViewBag.Suppliers = _supplierService.GetallAsync().Result.Data;

            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _purchaseService.DeletePurchaseAsync(id);
            if (result.Success)
            {
                TempData["success"] = "Purchase deleted successfully!";
            }
            else
            {
                TempData["error"] = result.Error;
            }
            return RedirectToAction("Index");
        }
    }
}
