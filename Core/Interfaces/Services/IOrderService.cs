using Core.Entities;
using Core.Specifications;

namespace Core.Interfaces.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Classroom>> GetMyLearningByStudentIdAsync(int studentId, ClassroomStatus? classStatus = null);
        Task<Order> CreateOrderFromCartAsync(string paymentIntentId, int studentId);
        Task<Order> GetOrderByPaymentIntentIdAsync(string paymentIntentId);
        Task<IEnumerable<Order>> GetAllOrdersAsync(ISpecification<Order> specification);
        Task UpdateOrderStatusAsync(Order order, OrderStatus orderStatus);
    }
}
