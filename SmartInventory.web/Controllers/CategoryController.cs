using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartInventory.BLL.Inteface;
using SmartInventory.Contract.Request;
using SmartInventory.Model;

namespace SmartInventory.web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {

        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
            
        }
        public async Task<IActionResult> Index()
        {
            var result = await _categoryService.GetallAsync();

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Error;
                return View(new List<Category>());
            }

            return View(result.Data);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryRequest category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            var result = await _categoryService.AddAsync(category);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Category created successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = result.Error;
                return View(category);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var categoryresult = await _categoryService.GetByIdAsync(id);
           if(!categoryresult.Success || categoryresult.Data==null)
            {
                TempData["ErrorMessage"] = categoryresult.Error ?? "Category not found.";
                return RedirectToAction("Index");
            }

           var category = categoryresult.Data;
            var updateRequest = new UpdateCategoryRequest
            {
                id = category.id,
                CategoryName = category.CategoryName,
                Description = category.Description
            };
            
            return View(updateRequest);


        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateCategoryRequest category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            var result = await _categoryService.UpdateAsync(category);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Category updated successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = result.Error ?? "An error occured when updating the category";
                return View(category);
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var categoryresult = await _categoryService.GetByIdAsync(id);
            if (!categoryresult.Success || categoryresult.Data == null)
            {
                TempData["ErrorMessage"] = categoryresult.Error ?? "Category not found.";
                return RedirectToAction("Index");
            }
            var category = categoryresult.Data;
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteAsync(id);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Category deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = result.Error ?? "An error occurred while deleting the category.";
            }
            return RedirectToAction("Index");
        }



    }
}
