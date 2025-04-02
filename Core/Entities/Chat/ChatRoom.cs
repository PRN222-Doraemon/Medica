namespace Core.Entities.Chat
{
    public class ChatRoom
    {
        public string ChatRoomId { get; set; }
        public DateTime CreateAt { get; set; }
        public List<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    }
}