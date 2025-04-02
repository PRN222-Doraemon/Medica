using Core.Constants;
using Core.Entities.Chat;
using Core.Interfaces;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace MedicaWeb_MVC.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(IChatService chatService, ILogger<ChatHub> logger)
        {
            _chatService = chatService;
            _logger = logger;
        }

        public async Task SendMessage(int senderId, int receiverId, string message)
        {
            try
            {
                _logger.LogInformation("Sending message from {SenderId} to {ReceiverId}", senderId, receiverId);

                // Send the message
                await _chatService.SendMessageAsync(senderId, receiverId, message);

                // Get the chat room ID
                var chatRoomId = string.Format(AppCts.RedisDatabase.Chat.KeyTemplate, senderId, receiverId);

                // Get the updated chat room
                var chatRoom = await _chatService.GetChatRoomByIdAsync(chatRoomId);

                // Send message to both sender and receiver using their connection IDs
                await Clients.User(senderId.ToString()).SendAsync("ReceiveMessage", chatRoom);
                await Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", chatRoom);

                // Also send to groups to ensure message delivery
                await Clients.Group(senderId.ToString()).SendAsync("ReceiveMessage", chatRoom);
                await Clients.Group(receiverId.ToString()).SendAsync("ReceiveMessage", chatRoom);

                _logger.LogInformation("Message sent successfully to both users");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message from {SenderId} to {ReceiverId}", senderId, receiverId);
                throw;
            }
        }

        public async Task JoinChat(int userId)
        {
            try
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());
                _logger.LogInformation("User {UserId} joined chat", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error joining chat for user {UserId}", userId);
                throw;
            }
        }

        public async Task LoadChatHistory(int userId1, int userId2)
        {
            try
            {
                var chatRoomId = string.Format(AppCts.RedisDatabase.Chat.KeyTemplate, userId1, userId2);
                var chatRoom = await _chatService.GetChatRoomByIdAsync(chatRoomId);

                if (chatRoom == null)
                {
                    chatRoom = new ChatRoom
                    {
                        ChatRoomId = chatRoomId,
                        CreateAt = DateTime.UtcNow,
                        Messages = new List<ChatMessage>()
                    };
                }

                await Clients.Caller.SendAsync("ReceiveMessage", chatRoom);
                _logger.LogInformation("Chat history loaded for users {UserId1} and {UserId2}", userId1, userId2);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading chat history for users {UserId1} and {UserId2}", userId1, userId2);
                throw;
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            try
            {
                var userId = Context.User?.FindFirst("UserId")?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
                    _logger.LogInformation("User {UserId} disconnected from chat", userId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling disconnection");
            }
            finally
            {
                await base.OnDisconnectedAsync(exception);
            }
        }
    }
}