namespace Core.Entities.Chat
{
    public class ChatMessage
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Message { get; set; }
        public bool IsSystemMessage { get; set; } = false; // For system message, like "User joined"
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    }
}
