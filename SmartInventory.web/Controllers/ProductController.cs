using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartInventory.BLL.Inteface;
using SmartInventory.Contract.Request;
using SmartInventory.Contract.Response;
using SmartInventory.DAL.Interface;
using SmartInventory.Model;

namespace SmartInventory.web.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        public ProductController(IProductService productService,ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }



        public IActionResult Index()
        {
          
          
            return View();
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> GetDataTables([FromForm] DataTablesRequest request)
        {
            if (request == null)
            {
                return BadRequest(new DataTablesResponse<Product>
                {
                    Draw = 0,
                    RecordsTotal = 0,
                    RecordsFiltered = 0,
                    Data = new List<Product>(),
                    Error = "Invalid request"
                });
            }

            var response = await _productService.GetDataTablesAsync(request);
            return Json(response);
        }


        public async Task<IActionResult> Create()
        {
            var categories=await _categoryService.GetallAsync();
            ViewBag.Categories = categories.Data ?? new List<Category>();
            return View(new CreateProductRequest());
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductRequest product)
        {
            if (ModelState.IsValid==false)
            {
                var categories = await _categoryService.GetallAsync();
                ViewBag.Categories = categories.Data;
                return View(product);
            }
            var result=await _productService.AddAsync(product);
            if (result.Success) {
                TempData["SuccessMessage"] = "Product Create Successfully";
                    return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = result.Error;
                return View(product);
            }
            
        }

        public async Task<IActionResult> Edit(int id)
        {
            var productResult = await _productService.GetByIdAsync(id);

            if (!productResult.Success || productResult.Data == null)
            {
                TempData["ErrorMessage"] = productResult.Error ?? "Product not found.";
                return RedirectToAction("Index");
            }

            var product = productResult.Data;
            var updateRequest = new UpdateProductRequest
            {
                id = product.id,
                Name = product.ProductName,
                Description = product.Description,
                Price = product.Price,
                StockQuantit = product.StockQuantit,
                CategoryId = product.CategoryId
            };
            var categories = await _categoryService.GetallAsync();
            ViewBag.Categories = categories.Data ?? new List<Category>();


            return View(updateRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateProductRequest product)
        {
            if (ModelState.IsValid == false)
            {
                var categories = await _categoryService.GetallAsync();
                ViewBag.Categories = categories.Data ?? new List<Category>();

                return View(product);
            }

            var result = await _productService.UpdateAsync(product);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Product updated successfully!";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = result.Error ?? "An error occurred while updating the product.";
            return View(product);
        }
        public async Task<IActionResult> Details(int id)
        {
            var productResult = await _productService.GetByIdAsync(id);

            if (!productResult.Success || productResult.Data == null)
            {
                TempData["ErrorMessage"] = productResult.Error ?? "Product not found.";
                return RedirectToAction("Index");
            }

            return View(productResult.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {

            var result = await _productService.DeleteAsync(id);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Product deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = result.Error ?? "An error occurred while deleting the product.";
            }
            return RedirectToAction("Index");
        }
    }

}
