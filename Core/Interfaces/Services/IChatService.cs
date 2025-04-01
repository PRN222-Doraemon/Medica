using Core.Entities.Chat;

namespace Core.Interfaces.Services
{
    public interface IChatService
    {
        /// <summary>
        /// Get message history within 1 chat room
        /// </summary>
        /// <param name="chatRoomId"></param>
        /// <returns></returns>
        public Task<List<ChatMessage>> GetChatMessagesAsync(string chatRoomId);

        /// <summary>
        /// Send a message from the sender
        /// </summary>
        /// <param name="chatRoomId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Task SendMessageAsync(int studentId, int senderId, string message);

        /// <summary>
        /// Get all chat rooms within the chat for 
        /// </summary>
        /// <returns></returns>
        public Task<List<ChatRoom>> GetAllChatRoomAsync(int pageIndex, int pageSize);
    }
}
