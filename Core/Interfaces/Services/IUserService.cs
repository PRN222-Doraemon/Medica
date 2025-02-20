using Core.Entities.Identity;
using System.Security.Claims;

namespace Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<ApplicationUser> GetUserByClaims(ClaimsPrincipal principal);
    }
}
