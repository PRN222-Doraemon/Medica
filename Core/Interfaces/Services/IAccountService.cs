using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Core.Interfaces.Services
{
    public interface IAccountService
    {
        Task<ApplicationUser?> GetUserByClaimsAsync(ClaimsPrincipal principal);
        Task<List<ApplicationRole>> GetAllRolesAsync();
        Task<bool> LoginAsync(string username, string password, bool isPersistence);
        Task LoginGoogleAsync(ClaimsPrincipal principal);
        Task<IdentityResult> RegisterAsync(ApplicationUser user, string password);
        Task LogoutAsync();
    }
}
