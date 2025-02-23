using Core.Entities;
using Core.Interfaces.Repos;
using Core.Interfaces.Services;
using Core.Specifications.Orders;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Order>> GetOrdersByStudentId(int studentId)
        {
            var spec = new OrderSpecification(new OrderParams { StudentID = studentId });
            return await _unitOfWork.Repository<Order>().ListAsync(spec);
        }
    }
}
