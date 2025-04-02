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

        public async Task<ChatRoom> GetOrCreateChatRoomAsync(int senderId, int receiverId)
        {
            var key = string.Format(AppCts.RedisDatabase.Chat.KeyTemplate, senderId, receiverId);
            var chatRoomJson = await _redisDb.StringGetAsync(key);

            if (chatRoomJson.IsNullOrEmpty)
            {
                var chatRoom = new ChatRoom
                {
                    ChatRoomId = key,
                    CreateAt = DateTime.UtcNow,
                    Messages = new List<ChatMessage>()
                };
                await _redisDb.StringSetAsync(
                    new RedisKey(key),
                    new RedisValue(JsonSerializer.Serialize(chatRoom)),
                    TimeSpan.FromDays(AppCts.RedisDatabase.Chat.ExpiryDays));
                return chatRoom;
            }

            return JsonSerializer.Deserialize<ChatRoom>(chatRoomJson);
        }

        public async Task<ChatRoom> GetChatMessagesAsync(string chatRoomId)
        {
            var chatRoomJson = await _redisDb.StringGetAsync(chatRoomId);
            if (chatRoomJson.IsNullOrEmpty)
            {
                return new ChatRoom { ChatRoomId = chatRoomId, Messages = new List<ChatMessage>() };
            }

            return JsonSerializer.Deserialize<ChatRoom>(chatRoomJson);
        }


        public async Task SendMessageAsync(int senderId, int receiverId, string message)
        {
            var chatRoom = await GetOrCreateChatRoomAsync(senderId, receiverId);
            var newMessage = new ChatMessage
            {
                MessageId = Guid.NewGuid().ToString(),
                Message = message,
                SenderId = senderId,
                ReceiverId = receiverId,
                TimeStamp = DateTime.UtcNow,
                IsSystemMessage = false
            };

            chatRoom.Messages.Add(newMessage);
            await _redisDb.StringSetAsync(
                new RedisKey(chatRoom.ChatRoomId),
                new RedisValue(JsonSerializer.Serialize(chatRoom)),
                TimeSpan.FromDays(AppCts.RedisDatabase.Chat.ExpiryDays));
        }

        public async Task<ChatRoom> GetChatRoomByIdAsync(string chatRoomId)
        {
            var chatRoomJson = await _redisDb.StringGetAsync(chatRoomId);
            if (chatRoomJson.IsNullOrEmpty)
            {
                return null;
            }

            return JsonSerializer.Deserialize<ChatRoom>(chatRoomJson);
        }


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
    }
}
