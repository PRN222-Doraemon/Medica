using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Classroom>> GetMyLearningByStudentId(int studentId, ClassroomStatus? classStatus);
        Task<Order> CreateOrderFromCartAsync(string paymentIntentId, int studentId);
        Task<Order> GetOrderByPaymentIntentId(string paymentIntentId);
        Task UpdateOrderStatusAsync(Order order, OrderStatus orderStatus);
        Task<IReadOnlyList<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(int orderId);
        Task<List<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
