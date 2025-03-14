using AutoMapper;
using Core.Constants;
using Core.Entities.Identity;
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
        private readonly IMapper _mapper;

        // ==============================
        // === Constructors
        // ==============================
        public AccountsController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        // ==============================
        // === Methods
        // ==============================

        [HttpGet]
        public IActionResult Login([FromQuery] string? returnUrl = null)
        {
            if (_accountService.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Courses");
            }

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
            return View(nameof(Login));
        }

        [HttpGet]
        public IActionResult Register([FromQuery] string? returnUrl = null)
        {
            if (_accountService.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Courses");
            }

            // Set returnUrl if accessing the authorize view
            returnUrl ??= Url.Content("~/");

            RegisterVM registerVM = new()
            {
                ReturnUrl = returnUrl,
            };
            return View(registerVM);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                var applicationUser = _mapper.Map<RegisterVM, ApplicationUser>(registerVM);

                // Only create account for student
                if (await _accountService.RegisterAsync(applicationUser, registerVM.Password, AppCts.Roles.Student))
                {
                    if (string.IsNullOrEmpty(registerVM.ReturnUrl))
                    {
                        return RedirectToAction("Index", "Courses");
                    }
                    else
                    {
                        TempData["error"] = "Register fail! Try again";
                        LocalRedirect(registerVM.ReturnUrl);
                    }
                }
            }

            return View(registerVM);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
