using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface ICartService
    {
        Task AddOrUpdateToCartAsync(int classRoomId, int userId);
        Task DeleteCartAsync(int userId);
        Task<List<CartItem>> GetCartItemsAsync(int userId);
    }
}
