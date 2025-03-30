using Core.Entities.Identity;
using Core.Specifications;
using System.Security.Claims;

namespace Core.Interfaces.Services
{
    public interface IAccountService
    {
        Task<ApplicationUser?> GetUserByClaimsAsync(ClaimsPrincipal principal);
        Task<List<ApplicationRole>> GetAllRolesAsync();
        Task<List<ApplicationUser>> GetAllRegisteredUserAsync(ISpecification<ApplicationUser> userSpecification);
        Task<int> GetTotalUsersCountAsync(ISpecification<ApplicationUser> userSpecification);
        Task<bool> LoginAsync(string username, string password, bool isPersistence);
        Task<bool> LoginGoogleAsync(ClaimsPrincipal principal);
        Task<bool> RegisterAsync(ApplicationUser user, string password, string role);
        bool IsSignedIn(ClaimsPrincipal principal);
        Task LogoutAsync();
    }
}
