using CloudinaryDotNet;
using Core.Interfaces.Services;
using MedicaWeb_MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicaWeb_MVC.Controllers
{
    public class AccountsController : Controller
    {
        // ==============================
        // === Fields & Props
        // ==============================

        private readonly IAccountService _accountService;

        // ==============================
        // === Constructors
        // ==============================

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // ==============================
        // === Methods
        // ==============================

        [HttpGet]
        public IActionResult Login([FromQuery] string? returnUrl = null)
        {
            // Set returnUrl if accessing the authorize view
            returnUrl ??= Url.Content("~/");

            LoginVM loginVM = new()
            {
                ReturnUrl = returnUrl,
            };
            return View(loginVM);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginVM loginVM)
        {
            var loginResult = await _accountService.LoginAsync(loginVM.UserName, loginVM.Password, loginVM.IsRememberMe);
            if (loginResult)
            {
                if (string.IsNullOrEmpty(loginVM.ReturnUrl))
                {
                    return RedirectToAction("Index", "Courses");
                }
                else
                {
                    return LocalRedirect(loginVM.ReturnUrl);
                }
            }

            TempData["error"] = "Invalid email or password.";
            return RedirectToAction(nameof(Login));
        }
    }
}
