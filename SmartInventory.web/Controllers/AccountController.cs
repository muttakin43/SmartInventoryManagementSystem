using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartInventory.Contract.Request.Account;
using SmartInventory.Model;

namespace SmartInventory.web.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            UserManager<ApplicationUser>userManager,
            SignInManager<ApplicationUser>signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult LogIn(string? returnurl= null)
        {
            ViewData["ReturnUrl"] = returnurl;
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LoginModel model , string? returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            if (ModelState.IsValid==false)
            {
                return View(model);
            }
            var result= await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,false);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Login successfully.";
                return RedirectToLocal(returnurl);

            }
            TempData["ErrorMessage"] = "Invalid login attempt.";
            return View(model);
        }


        public IActionResult Register()
                    {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);

            }
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName
                , LastName = model.LastName,
                IsActive = true
                , CreatedAt = DateTime.Now
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");

                await _signInManager.SignInAsync(user, isPersistent: false);
                TempData["SuccessMessage"] = "Registration successful. You can now log in.";
                return RedirectToAction("LogIn", "Account");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        
           
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            TempData["SuccessMessage"] = "You have been loged out.";
            return RedirectToAction("Index", "Home");   
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        private IActionResult RedirectToLocal(string? returnurl)
        {
            if (Url.IsLocalUrl(returnurl))
            {
                return Redirect(returnurl);

            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
