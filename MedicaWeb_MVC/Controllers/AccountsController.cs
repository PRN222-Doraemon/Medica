using Core.Interfaces.Services;
using MedicaWeb_MVC.ViewModels;
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
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginVM loginVM)
        {
            var loginResult = await _accountService.LoginAsync(loginVM.UserName, loginVM.Password, loginVM.IsRememberMe);
            if (loginResult)
            {
                return RedirectToAction("Index", "Courses");
            }

            TempData["Error"] = "Invalid email or password.";
            return RedirectToAction(nameof(Login));
        }
    }
}
