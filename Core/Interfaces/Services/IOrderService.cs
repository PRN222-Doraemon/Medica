using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Classroom>> GetMyLearningByStudentIdAsync(int studentId, ClassroomStatus? classStatus);
        Task<Order> CreateOrderFromCartAsync(string paymentIntentId, int studentId);
        Task<Order> GetOrderByPaymentIntentIdAsync(string paymentIntentId);
        Task UpdateOrderStatusAsync(Order order, OrderStatus orderStatus);
    }
}
