using Microsoft.AspNetCore.Mvc;
using SmartInventory.BLL.Inteface;
using SmartInventory.Contract.Request;
using SmartInventory.DAL.Interface;
using SmartInventory.Model;

namespace SmartInventory.web.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierService;
        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _supplierService.GetallAsync();

            return View(result.Data ?? new List<Supplier>());
        }


        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSupplierRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var result = await _supplierService.AddAsync(request);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Supplier added successfully.";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = result.Error;
            return View(request);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var supplierresult = await _supplierService.GetByIdAsync(id);
            if (!supplierresult.Success || supplierresult.Data == null)
            {
                TempData["ErrorMessage"] = supplierresult.Error ?? "Supplier not found";
                return RedirectToAction("Index");
            }
            var supplier = supplierresult.Data;
            var updateRequest = new UpdateSupplierRequest
            {
                id = supplier.id,
                Name = supplier.Name,
                Phone = supplier.Phone,
                Address = supplier.Address
            };
            return View(updateRequest);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UpdateSupplierRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var result = await _supplierService.UpdateAsync(request);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Supplier updated successfully.";
                return RedirectToAction("Index");
            }
            TempData["ErrorMessage"] = result.Error ?? "An error occured when updating the supplier";
            return View(request);

        }

        public async Task<IActionResult> Details(int id)
        {
            var supplierresult = await _supplierService.GetByIdAsync(id);
            if (!supplierresult.Success || supplierresult.Data == null)
            {
                TempData["ErrorMessage"] = supplierresult.Error ?? "Supplier not found";
                return RedirectToAction("Index");
            }
            var supplier = supplierresult.Data;
            return View(supplier);

        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _supplierService.DeleteAsync(id);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Supplier deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = result.Error ?? "An error occured when deleting the supplier";
            }
            return RedirectToAction("Index");
        }
    }
}