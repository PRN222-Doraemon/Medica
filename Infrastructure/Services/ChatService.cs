using Core.Constants;
using Core.Entities.Chat;
using Core.Interfaces.Services;
using StackExchange.Redis;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class ChatService : IChatService
    {
        // ==============================
        // === Fields & Props
        // ==============================

        private readonly IDatabase _redisDb;

        // ==============================
        // === Constructors
        // ==============================

        public ChatService(IConnectionMultiplexer connectionMultiplexer)
        {
            _redisDb = connectionMultiplexer.GetDatabase(AppCts.RedisDatabase.Chat.Database);
        }

        // ==============================
        // === Methods
        // ==============================

        public async Task<List<ChatRoom>> GetAllChatRoomAsync(int pageIndex, int pageSize)
        {
            var server = _redisDb.Multiplexer.GetServer(_redisDb.Multiplexer.GetEndPoints()[0]);
            var keys = server.Keys(
                AppCts.RedisDatabase.Chat.Database,
                AppCts.RedisDatabase.Chat.KeyTemplate + "*",
                pageSize,
                0,
                (pageIndex - 1) * pageSize);

            var chatRooms = new List<ChatRoom>();
            foreach (var key in keys)
            {
                var chatRoomJson = await _redisDb.StringGetAsync(key);
                if (chatRoomJson.IsNullOrEmpty) continue;

                var chatRoom = JsonSerializer.Deserialize<ChatRoom>(chatRoomJson);
                chatRooms.Add(chatRoom);
            }

            return chatRooms;
        }

        public async Task<List<ChatMessage>> GetChatMessagesAsync(string chatRoomId)
        {
            // Get a string object
            var chatRoomJson = await _redisDb.StringGetAsync(chatRoomId);
            if (chatRoomJson.IsNullOrEmpty) return new List<ChatMessage>();

            // Deserialize into object
            var chatRoom = JsonSerializer.Deserialize<ChatRoom>(chatRoomJson);
            return chatRoom.Messages;
        }

        public async Task SendMessageAsync(int senderId, int receiverId, string message)
        {
            var key = string.Format(AppCts.RedisDatabase.Cart.KeyTemplate, senderId, receiverId);

            var chatRoomJson = await _redisDb.StringGetAsync(key);

            // Create new chat room if sending message
            ChatRoom chatRoom = chatRoomJson.IsNullOrEmpty
                ? new ChatRoom() { ChatRoomId = key, CreateAt = DateTime.Now, Messages = new List<ChatMessage>() }
                : JsonSerializer.Deserialize<ChatRoom>(chatRoomJson);

            // Add message to the new room
            chatRoom.Messages.Add(new ChatMessage()
            {
                MessageId = Guid.NewGuid().ToString(),
                Message = message,
                SenderId = senderId,
                ReceiverId = receiverId,
            });

            // Serialize and add to redis
            await _redisDb.StringSetAsync(
                new RedisKey(key),
                new RedisValue(JsonSerializer.Serialize(chatRoom)),
                TimeSpan.FromDays(AppCts.RedisDatabase.Chat.ExpiryDays));
        }
    }
}
