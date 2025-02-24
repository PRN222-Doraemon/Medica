using Core.Entities.Identity;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        // =========================
        // === Fields & Props
        // =========================

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        // =========================
        // === Constructors
        // =========================
        public AccountService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        // =========================
        // === Methods
        // =========================

        public async Task<ApplicationUser?> GetUserByClaimsAsync(ClaimsPrincipal principal)
        {
            return await _userManager.GetUserAsync(principal);
        }

        public async Task<bool> LoginAsync(string username, string password, bool isPersistent)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return false;

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded) return false;

            await _signInManager.SignInAsync(user, isPersistent);
            return true;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> RegisterAsync(ApplicationUser user, string password, string role)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded && user != null) return false;

            var roleAssigned = await _userManager.AddToRoleAsync(user, role);
            // Rollback if add role fail
            if (!roleAssigned.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                return false;
            }

            // Add claims for further retrival
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, role)
            };

            var claimResult = await _userManager.AddClaimsAsync(user, claims);

            // Rollback if add claims fail
            if (!claimResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                return false;
            }

            return true;
        }

        public async Task<List<ApplicationRole>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        public Task<bool> LoginGoogleAsync(ClaimsPrincipal principal)
        {
            return null;
        }

        public bool IsSignedIn(ClaimsPrincipal principal)
        {
            if (principal == null) return false;
            return _signInManager.IsSignedIn(principal);
        }
    }
}
