using Core.Constants;
using Core.Entities.Chat;
using Core.Interfaces.Services;
using StackExchange.Redis;

namespace Infrastructure.Services
{
    public class ChatService : IChatService
    {
        // ==============================
        // === Fields & Props
        // ==============================

        private readonly IChatService _chatService;
        private readonly IDatabase _redisDb;

        // ==============================
        // === Constructors
        // ==============================

        public ChatService(IChatService chatService, IConnectionMultiplexer connectionMultiplexer)
        {
            _chatService = chatService;
            _redisDb = connectionMultiplexer.GetDatabase(AppCts.RedisDatabase.Chat);
        }

        // ==============================
        // === Methods
        // ==============================

        public Task<List<ChatRoom>> GetAllChatRoomAsync(int pageIndex, int pageSize)
        {

        }

        public Task<List<ChatMessage>> GetChatMessagesAsync(string chatRoomId)
        {
        }

        public Task SendMessageAsync(int studentId, int senderId, string message)
        {
        }
    }
}
