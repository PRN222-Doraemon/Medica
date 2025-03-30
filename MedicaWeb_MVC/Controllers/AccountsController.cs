using AutoMapper;
using Core.Constants;
using Core.Entities.Identity;
using Core.Interfaces.Services;
using MedicaWeb_MVC.ViewModels.User;
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
        private readonly ICloudinaryService _cloudinaryService;

        // ==============================
        // === Constructors
        // ==============================
        public AccountsController(IAccountService accountService, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _accountService = accountService;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
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
                if (string.IsNullOrEmpty(loginVM.ReturnUrl) || loginVM.ReturnUrl.Equals("/"))
                {
                    // If admin, redirect to dashboard
                    if (User.IsInRole(AppCts.Roles.Admin))
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }

                    // Else, redirect to Courses
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

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _accountService.GetUserByClaimsAsync(User);

            var viewModel = new ProfilePageVM
            {
                ProfileData = new ProfileVM
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                    Email = user.Email,
                    ProfileImageUrl = user.ImageUrl,
                    DateOfBirth = user.DateOfBirth,
                }
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfileImage(IFormFile profileImage)
        {
            try
            {
                if (profileImage == null || profileImage.Length == 0)
                {
                    TempData["error"] = "Please select an image to upload.";
                    return RedirectToAction(nameof(Profile));
                }

                var user = await _accountService.GetUserByClaimsAsync(User);
                if (user == null)
                {
                    TempData["error"] = "User not found";
                    return RedirectToAction(nameof(Profile));
                }


                var imageUrl = await _cloudinaryService.UploadAsync(profileImage);

                if (user.ImageUrl != null)
                {
                    await _cloudinaryService.DeleteImageAsync(user.ImageUrl);
                }

                user.ImageUrl = imageUrl;
                await _accountService.UpdateUserAsync(user);

                return RedirectToAction(nameof(Profile));
            }
            catch (Exception ex)
            {
                TempData["error"] = "Failed to upload image. Please try again.";
                return RedirectToAction(nameof(Profile));
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ProfilePageVM model)
        {
            var user = await _accountService.GetUserByClaimsAsync(User);
            if (user == null)
            {
                TempData["error"] = "User not found.";
                return RedirectToAction(nameof(Profile));
            }

            var result = await _accountService.ChangePasswordAsync(user, model.PasswordData.CurrentPassword, model.PasswordData.NewPassword);
            if (result)
            {
                TempData["success"] = "Password changed successfully!";
                return RedirectToAction(nameof(Profile));
            }

            TempData["error"] = "Invalid current password.";
            return RedirectToAction(nameof(Profile));
        }
    }
}
