using Core.Entities.Chat;

namespace Core.Interfaces.Services
{
    public interface IChatService
    {
        Task<ChatRoom> GetChatRoomByIdAsync(string chatRoomId);
        Task<List<ChatRoom>> GetAllChatRoomAsync(int pageIndex, int pageSize);
        Task<ChatRoom> GetChatMessagesAsync(string chatRoomId);
        Task SendMessageAsync(int senderId, int receiverId, string message);

    }
}
