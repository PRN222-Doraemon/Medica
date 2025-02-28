using Core.Entities;
using Core.Interfaces.Repos;
using Core.Interfaces.Services;
using Core.Specifications.Orders;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        // ==============================
        // === Fields & Props
        // ==============================

        private readonly IUnitOfWork _unitOfWork;

        // ==============================
        // === Constructors
        // ==============================

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ==============================
        // === Methods
        // ==============================

        public async Task<IEnumerable<Classroom>> GetMyLearningByStudentId(int studentId, ClassroomStatus? classroomStatus)
        {
            var spec = new OrderSpecification(new OrderParams { StudentID = studentId });
            var orders = await _unitOfWork.Repository<Order>().ListAsync(spec);
            var orderDetails = orders.SelectMany(o => o.OrderDetails).ToList();
            var classes = orderDetails.Select(od => od.Classroom).ToList();
            if (classroomStatus.HasValue)
            {
                switch (classroomStatus)
                {
                    case ClassroomStatus.Upcoming:
                        classes = classes.Where(c => c.StartDate > DateOnly.FromDateTime(DateTime.UtcNow)).ToList();
                        break;
                    case ClassroomStatus.Ongoing:
                        classes = classes.Where(c => c.StartDate <= DateOnly.FromDateTime(DateTime.UtcNow)
                                  && c.EndDate > DateOnly.FromDateTime(DateTime.UtcNow)).ToList();
                        break;
                    case ClassroomStatus.Completed:
                        classes = classes.Where(c => c.EndDate < DateOnly.FromDateTime(DateTime.UtcNow)).ToList();
                        break;
                }
            }
            return classes;
        }

        public Task<Order> CreateOrderFromCartAsync(int userId, string sessionId)
        {
            throw new NotImplementedException();
        }


        public Task<Order> GetOrderByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
