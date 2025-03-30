using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface ICartService
    {
        Task AddOrUpdateToCartAsync(int classRoomId, int userId);
        Task DeleteCartAsync(int userId);
        Task<List<CartItem>> GetCartItemsAsync(int userId);
    }
}
