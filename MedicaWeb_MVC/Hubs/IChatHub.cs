using Core.Entities.Chat;

namespace MedicaWeb_MVC.Hubs
{
    public interface IChatHub
    {
        Task ReceiveMessage(ChatRoom chatRoom);
    }
}