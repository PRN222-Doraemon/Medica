using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<string> CreateCheckoutSessionAsync(List<CartItem> cartItems, int userId, string successfulUrl, string cancelUrl);
    }
}
