using Core.Constants;
using Core.Interfaces.Services;
using MedicaWeb_MVC.ViewModels.VideoCall;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MedicaWeb_MVC.Controllers
{
    public class VideoCallController : Controller
    {
        private readonly IAccountService _accountService;

        public VideoCallController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("/VideoCall/{id}")]
        public async Task<IActionResult> IndexAsync(string? id)
        {
            var user = await _accountService.GetUserByClaimsAsync(User);
            var fullName = user?.FullName;
            var roleName = User.IsInRole(AppCts.Roles.Lecturer) ? "Lecturer" : "Student";
            var userName = user?.UserName;
            var vm = new VideoCallVM
            {
                FullName = fullName,
                RoleName = roleName,
                UserName = userName,
                RoomId = id
            };
            return View(vm);
        }

    }
}
