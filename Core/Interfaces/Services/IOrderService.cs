using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Classroom>> GetMyLearningByStudentId(int studentId, ClassroomStatus? classStatus);
        Task<Order> GetOrderByIdAsync(int id);
        Task<Order> CreateOrderFromCartAsync(int studentId, string sessionId);
    }
}
