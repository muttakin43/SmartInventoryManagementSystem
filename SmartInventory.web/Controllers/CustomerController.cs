using Microsoft.AspNetCore.Mvc;
using SmartInventory.BLL.Inteface;
using SmartInventory.Contract.Request;

namespace SmartInventory.web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // 🔵 LIST
        public async Task<IActionResult> Index()
        {
            var result = await _customerService.GetAllAsync();

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Error;
                return View(new List<CustomerResponse>());
            }

            return View(result.Data);
        }

        // 🔵 DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var result = await _customerService.GetByIdAsync(id);

            if (!result.Success)
            {
                return NotFound(result.Error);
            }

            return View(result.Data);
        }

        // 🔵 CREATE GET
        public IActionResult Create()
        {
            return View();
        }

        // 🔵 CREATE POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _customerService.CreateAsync(request);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Error;
                return View(request);
            }

            return RedirectToAction(nameof(Index));
        }

        // 🔵 EDIT GET
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _customerService.GetByIdAsync(id);

            if (!result.Success)
                return NotFound(result.Error);

            var model = new CustomerCreateRequest
            {
                Name = result.Data.Name,
                Address = result.Data.Address,
                PhoneNumber = result.Data.PhoneNumber
            };

            ViewBag.CustomerId = id;

            return View(model);
        }

        // 🔵 EDIT POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CustomerCreateRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _customerService.UpdateAsync(id, request);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Error;
                return View(request);
            }

            return RedirectToAction(nameof(Index));
        }

        // 🔵 DELETE
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _customerService.DeleteAsync(id);

            if (!result.Success)
                return BadRequest(result.Error);

            return RedirectToAction(nameof(Index));
        }
    }
}