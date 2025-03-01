using Core.Entities;
using Core.Interfaces.Repos;
using Core.Interfaces.Services;
using Core.Specifications.Orders;
using Stripe.Checkout;

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

        public Task<Order> UpdateOrderStatus(int orderId, OrderStatus orderStatus, string paymentIntentId)
        {
            throw new NotImplementedException();
        }
        public async Task<Order> CreateOrderFromCartAsync(int studentId, string paymentIntentId)
        {
            // Get item from cart
            var cartItems = await _cartService.GetCartItemsAsync(studentId);
            if (cartItems == null || !cartItems.Any()) throw new InvalidOperationException("Cart is empty");

            // Create new order
            var order = new Order
            {
                StudentId = studentId,
                PaymentIntentId = paymentIntentId,
                Status = OrderStatus.Pending,
                OrderTime = DateTime.UtcNow,
                TotalPrice = cartItems.Select(o => o.Price).Sum(),
                OrderDetails = cartItems.Select(item => new OrderDetail
                {
                    ClassroomID = item.ClassRoomId,
                    Price = item.Price
                }).ToList()
            };

            // Commit and Delete cart if commit successfully
            _unitOfWork.Repository<Order>().Add(order);
            if (await _unitOfWork.CompleteAsync() > 0)
            {
                await _cartService.DeleteCartAsync(studentId);
            }
            else
            {
                throw new InvalidOperationException("Failed to create order.");
            }

            return order;
        }
    }
}
