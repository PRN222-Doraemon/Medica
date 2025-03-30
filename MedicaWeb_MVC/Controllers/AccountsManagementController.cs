using AutoMapper;
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

            var accountVM = _mapper.Map<ApplicationUser, AccountVM>(user);
            accountVM.RoleName = roleName;

            ViewBag.Roles = await _roleManager.Roles.ToListAsync();
            return View(accountVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AccountVM model)
        {
            if (id != model.Id)
            {
                TempData["error"] = "Invalid account ID.";
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user == null)
                {
                    TempData["error"] = "Account not found.";
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
                        TempData["error"] = "Image size must be less than 5MB.";
                        return RedirectToAction(nameof(Index), "AccountsManagement");
                    }

                    // Validate file type
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var fileExtension = Path.GetExtension(model.ImageFile.FileName).ToLowerInvariant();
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        TempData["error"] = "Only JPG, PNG, and GIF files are allowed.";
                        return RedirectToAction(nameof(Index), "AccountsManagement");
                    }

                    try
                    {
                        // Upload new image to Cloudinary
                        user.ImageUrl = await _cloudinaryService.UploadAsync(model.ImageFile);
                    }
                    catch (Exception ex)
                    {
                        TempData["error"] = $"Failed to upload image: {ex.InnerException?.Message ?? ex.Message}";
                        return RedirectToAction(nameof(Index), "AccountsManagement");
                    }
                }

                var result = await _userManager.UpdateAsync(user);

                // Checking if result successfull
                if (result.Succeeded)
                {
                    try
                    {
                        // Update role if changed
                        var currentRoles = await _userManager.GetRolesAsync(user);
                        if (currentRoles.FirstOrDefault() != model.RoleName)
                        {
                            await _userManager.RemoveFromRolesAsync(user, currentRoles);
                            await _userManager.AddToRoleAsync(user, model.RoleName);
                        }

                        TempData["success"] = "Account updated successfully";
                        return RedirectToAction(nameof(Index), "AccountsManagement");
                    }
                    catch (Exception ex)
                    {
                        TempData["error"] = $"Failed to update role: {ex.InnerException?.Message ?? ex.Message}";
                        return RedirectToAction(nameof(Index), "AccountsManagement");
                    }
                }
            }
            else
            {
                TempData["error"] = "Please correct the validation errors.";
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
        public async Task<IActionResult> Create(AccountVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Status = model.Status,
                    DateOfBirth = model.DateOfBirth,
                };

                var result = await _userManager.CreateAsync(user, "Password123!"); // You might want to generate a random password or let the admin set it

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.RoleName);
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
    }
}