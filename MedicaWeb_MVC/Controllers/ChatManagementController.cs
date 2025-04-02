using Core.Entities.Chat;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicaWeb_MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ChatManagementController : Controller
    {
        private readonly IChatService _chatService;

        public ChatManagementController(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task<IActionResult> Index()
        {
            var chatRooms = await _chatService.GetAllChatRoomAsync(1, 100); // Get first 100 chat rooms
            return View(chatRooms);
        }

        [HttpGet]
        public async Task<IActionResult> GetChatMessages(string chatRoomId)
        {
            var messages = await _chatService.GetChatMessagesAsync(chatRoomId);
            return Json(messages);
        }
    }
}