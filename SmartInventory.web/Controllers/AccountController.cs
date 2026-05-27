using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using SmartInventory.BLL.Inteface;
using SmartInventory.Contract.Request.Account;
using SmartInventory.Model;
using System.Text;

namespace SmartInventory.web.Controllers
{
    [Controller]
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public IActionResult LogIn(string? returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LoginModel model, string? returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            if (ModelState.IsValid == false)
            {
                return View(model);
            }
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
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
                ,
                LastName = model.LastName,
                IsActive = true
                ,
                CreatedAt = DateTime.Now
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
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgetPasswordModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            // User না থাকলেও same message দেখান
            if (user == null)
            {
                TempData["Message"] = "If this email exists, a reset link has been sent.";
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            // Identity নিজেই token তৈরি করবে
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            // Reset link তৈরি
            var resetLink = Url.Action("ResetPassword", "Account",
                new { token = token, email = model.Email },
                Request.Scheme);

            // Email body
            var body = $@"
        <div style='font-family:sans-serif;max-width:480px;margin:auto;
                    background:#0d1117;color:#c9d1d9;padding:2rem;border-radius:12px;'>
            <h2 style='color:#f0f6fc;'>Reset Your Password</h2>
            <p>Click the button below to reset your password.</p>
            <p>This link will expire in <b>30 minutes</b>.</p>
            <a href='{resetLink}'
               style='display:inline-block;padding:12px 28px;background:#1f6feb;
                      color:#fff;border-radius:6px;text-decoration:none;
                      font-weight:600;margin:1rem 0;'>
                Reset Password
            </a>
            <p style='color:#6e7681;font-size:12px;margin-top:1.5rem;'>
                If you didn't request this, you can safely ignore this email.
            </p>
        </div>";

            await _emailService.SendAsync(model.Email, "Reset Your Inventra Password", body);

            TempData["Message"] = "If this email exists, a reset link has been sent.";
            return RedirectToAction("ForgotPasswordConfirmation");
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
                return RedirectToAction("Login");
            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var model = new ResetPasswordModel { Token = token, Email = email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid request.");
                return View(model);
            }

            // Identity নিজেই token verify করে password update করবে
            var result = await _userManager.ResetPasswordAsync(
                user, model.Token, model.NewPassword);

            if (result.Succeeded)
            {
                TempData["Success"] = "Password reset successful! Please sign in.";
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

      


    }
}
