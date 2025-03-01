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
        private readonly ICartService _cartService;

        // ==============================
        // === Constructors
        // ==============================

        public OrderService(IUnitOfWork unitOfWork, ICartService cartService)
        {
            _unitOfWork = unitOfWork;
            _cartService = cartService;
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

        //public async Task<Order> CreateOrderFromCartAsync(int userId)
        //{
        //    // 1. Get item from cart
        //    var cartItems = await _cartService.GetCartItemsAsync(userId);
        //    if (cartItems == null || !cartItems.Any()) throw new InvalidOperationException("Cart is empty");

        //    // 2. If successful than create order, add to the database
        //    var order = new Order() { 

        //    }

        //    // 3. Clearing the cart

        //    // 4. User is direct to the order summary
        //}

        //public Task<Order> GetOrderByIdAsync(int id)
        //{

        //}
    }
}
