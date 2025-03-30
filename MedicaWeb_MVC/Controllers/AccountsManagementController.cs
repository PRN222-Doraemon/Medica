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
        private readonly IAccountService _accountService;
        private const int PageSize = 10;

        // ==============================
        // === Constructors
        // ==============================

        public AccountsManagementController(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IAccountService accountService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _accountService = accountService;
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
                var role = await _roleManager.FindByNameAsync(roleName);

                accounts.Add(new AccountVM
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    RoleName = roleName,
                    Status = user.Status,
                    DateOfBirth = user.DateOfBirth,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                });
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

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return Json(new { success = false });
            }

            user.Status = user.Status == UserStatus.Enabled ? UserStatus.Disabled : UserStatus.Enabled;
            user.UpdatedAt = DateTime.Now;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false });
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

            var accountVM = new AccountVM
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                RoleName = roleName,
                Status = user.Status,
                DateOfBirth = user.DateOfBirth,
                ImageUrl = user.ImageUrl
            };

            ViewBag.Roles = await _roleManager.Roles.ToListAsync();
            return View(accountVM);
        }

        [HttpPost]
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

                user.UserName = model.Username;
                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.PhoneNumber;
                user.Status = model.Status;
                user.DateOfBirth = model.DateOfBirth;
                user.ImageUrl = model.ImageUrl;
                user.UpdatedAt = DateTime.Now;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    // Update role if changed
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    if (currentRoles.FirstOrDefault() != model.RoleName)
                    {
                        await _userManager.RemoveFromRolesAsync(user, currentRoles);
                        await _userManager.AddToRoleAsync(user, model.RoleName);
                    }

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
                    ImageUrl = model.ImageUrl,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                var result = await _userManager.CreateAsync(user, "DefaultPassword123!"); // You might want to generate a random password or let the admin set it

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