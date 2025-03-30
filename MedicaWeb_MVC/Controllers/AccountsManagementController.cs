using AutoMapper;
using Core.Constants;
using Core.Entities.Identity;
using Core.Interfaces.Services;
using Core.Specifications.Users;
using MedicaWeb_MVC.ViewModels.Shared;
using MedicaWeb_MVC.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicaWeb_MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccountsManagementController : Controller
    {
        // ==============================
        // === Fields & Props
        // ==============================

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly ICloudinaryService _cloudinaryService;
        private const int PageSize = 10;

        // ==============================
        // === Constructors
        // ==============================

        public AccountsManagementController(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IMapper mapper,
            IAccountService accountService,
            ICloudinaryService cloudinaryService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _accountService = accountService;
            _cloudinaryService = cloudinaryService;
            _mapper = mapper;
        }

        // ==============================
        // === Methods
        // ==============================

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] string search = "")
        {
            var userParams = new UserParams
            {
                Page = page,
                PageSize = PageSize,
                Search = search
            };

            var users = await _accountService.GetAllRegisteredUserAsync(new UserSpecification(userParams, true));
            var totalItems = await _accountService.GetTotalUsersCountAsync(new UserSpecification(userParams, false));
            var accounts = new List<AccountVM>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var roleName = roles.FirstOrDefault() ?? "No Role";

                // Create accountVM
                var accountVm = _mapper.Map<ApplicationUser, AccountVM>(user);
                accountVm.RoleName = roleName;

                accounts.Add(accountVm);
            }

            var model = new ListVM<AccountVM>
            {
                Items = accounts,
                PagingInfo = new PagingVM
                {
                    CurrentPage = page,
                    TotalItems = totalItems,
                    ItemsPerPage = PageSize
                },
                SearchValue = new SearchbarVM
                {
                    Controller = "AccountsManagement",
                    Action = "Index",
                    SearchText = search
                }
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var roleName = roles.FirstOrDefault() ?? "No Role";

            ViewBag.Roles = await _roleManager.Roles.ToListAsync();
            return View(_mapper.Map<ApplicationUser, AccountVM>(user));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AccountVM model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    return NotFound();
                }

                // Update basic user information
                _mapper.Map(model, user);

                // Handle image upload if a new image is provided
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    // Validate file size (5MB max)
                    if (model.ImageFile.Length > 5 * 1024 * 1024)
                    {
                        ModelState.AddModelError("", "Image size must be less than 5MB");
                        ViewBag.Roles = await _roleManager.Roles.ToListAsync();
                        return View(model);
                    }

                    // Validate file type
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var fileExtension = Path.GetExtension(model.ImageFile.FileName).ToLowerInvariant();
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError("", "Only JPG, PNG, and GIF files are allowed");
                        ViewBag.Roles = await _roleManager.Roles.ToListAsync();
                        return View(model);
                    }

                    // Upload new image to Cloudinary
                    user.ImageUrl = await _cloudinaryService.UploadAsync(model.ImageFile);
                }

                var result = await _userManager.UpdateAsync(user);

                // Checking if result successfull
                if (result.Succeeded)
                {
                    // Update role if changed
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    if (currentRoles.FirstOrDefault() != model.RoleName)
                    {
                        await _userManager.RemoveFromRolesAsync(user, currentRoles);
                        await _userManager.AddToRoleAsync(user, model.RoleName);
                    }

                    TempData["Success"] = "Account updated successfully";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ViewBag.Roles = await _roleManager.Roles.ToListAsync();
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Roles = _roleManager.Roles.ToList();
            return View(new AccountVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AccountVM model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = await _roleManager.Roles.ToListAsync();
                return View(model);
            }

            try
            {
                // Check if username or email already exists
                if (await _userManager.FindByNameAsync(model.Username) != null)
                {
                    TempData["error"] = "Username already exists.";
                    ViewBag.Roles = await _roleManager.Roles.ToListAsync();
                    return View(model);
                }

                if (await _userManager.FindByEmailAsync(model.Email) != null)
                {
                    TempData["error"] = "Email already exists.";
                    ViewBag.Roles = await _roleManager.Roles.ToListAsync();
                    return View(model);
                }

                // Validate image if provided
                string? imageUrl = null;
                if (model.ImageFile != null)
                {
                    if (model.ImageFile.Length > 5 * 1024 * 1024)
                    {
                        TempData["error"] = "Image size must be less than 5MB.";
                        ViewBag.Roles = await _roleManager.Roles.ToListAsync();
                        return View(model);
                    }

                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var fileExtension = Path.GetExtension(model.ImageFile.FileName).ToLowerInvariant();
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        TempData["error"] = "Only JPG, PNG, and GIF files are allowed.";
                        ViewBag.Roles = await _roleManager.Roles.ToListAsync();
                        return View(model);
                    }

                    // Upload image to Cloudinary
                    imageUrl = await _cloudinaryService.UploadAsync(model.ImageFile);
                }

                // Create user
                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Status = model.Status,
                    DateOfBirth = model.DateOfBirth,
                    ImageUrl = imageUrl
                };

                // Create user with default password
                var result = await _userManager.CreateAsync(user, AppCts.Accounts.DefaultPassword);

                if (!result.Succeeded)
                {
                    // If user creation fails, delete the uploaded image
                    if (imageUrl != null)
                    {
                        await _cloudinaryService.DeleteImageAsync(imageUrl);
                    }
                    TempData["error"] = string.Join(", ", result.Errors.Select(e => e.Description));
                    ViewBag.Roles = await _roleManager.Roles.ToListAsync();
                    return View(model);
                }

                // Assign role
                await _userManager.AddToRoleAsync(user, model.RoleName);
                TempData["success"] = "Account created successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["error"] = $"An error occurred while creating the account: {ex.InnerException?.Message ?? ex.Message}";
                ViewBag.Roles = await _roleManager.Roles.ToListAsync();
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    TempData["error"] = "User not found.";
                    return RedirectToAction(nameof(Index));
                }

                // Delete user's image from Cloudinary if exists
                if (!string.IsNullOrEmpty(user.ImageUrl))
                {
                    await _cloudinaryService.DeleteImageAsync(user.ImageUrl);
                }

                // Delete user
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    TempData["error"] = string.Join(", ", result.Errors.Select(e => e.Description));
                    return RedirectToAction(nameof(Index));
                }

                TempData["success"] = "Account deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["error"] = $"An error occurred while deleting the account: {ex.InnerException?.Message ?? ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}