using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Classroom>> GetMyLearningByStudentId(int studentId, ClassroomStatus? classStatus);
        Task<Order> CreateOrderFromCartAsync(int studentId, string paymentIntentId);
        Task<Order> UpdateOrderStatus(int orderId, OrderStatus orderStatus, string paymentIntentId);
    }
}
