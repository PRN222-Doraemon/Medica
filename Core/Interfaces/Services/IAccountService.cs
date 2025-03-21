﻿using Core.Entities.Identity;
using System.Security.Claims;

namespace Core.Interfaces.Services
{
    public interface IAccountService
    {
        Task<ApplicationUser?> GetUserByClaimsAsync(ClaimsPrincipal principal);
        Task<List<ApplicationRole>> GetAllRolesAsync();
        Task<bool> LoginAsync(string username, string password, bool isPersistence);
        Task<bool> LoginGoogleAsync(ClaimsPrincipal principal);
        Task<bool> RegisterAsync(ApplicationUser user, string password, string role);
        Task<List<ApplicationUser>> GetAllRegisteredUserAsync();
        bool IsSignedIn(ClaimsPrincipal principal);
        Task LogoutAsync();
    }
}
