using Core.Entities;

namespace Core.Interfaces.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrdersByStudentId(int studentId);
    }
}
