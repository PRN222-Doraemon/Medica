using AutoMapper;
using Core.Interfaces.Services;
using Core.Specifications.Orders;
using MedicaWeb_MVC.ViewModels.Orders;
using MedicaWeb_MVC.ViewModels.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicaWeb_MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrdersManagementController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private const int PageSize = 10;
        public OrdersManagementController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] int page = 1, [FromQuery] string search = "")
        {
            var orderParams = new OrderParams()
            {
                Page = page,
                Search = search,
                PageSize = PageSize
            };

            var orders = await _orderService.GetAllOrdersAsync(new OrderSpecification(orderParams, true));
            var totalOrders = await _orderService.GetAllOrdersAsync(new OrderSpecification(orderParams, false));
            var orderVMs = orders.Select(order => new OrderVM
            {
                Id = order.Id,
                StudentId = order.StudentId,
                StudentName = order.Student != null ? $"{order.Student.FirstName} {order.Student.LastName}" : "N/A",
                OrderTime = order.OrderTime,
                Status = order.Status,
                PaymentIntentId = order.PaymentIntentId,
                TotalPrice = order.TotalPrice,
            }).ToList();

            var columns = new Dictionary<string, string>
            {
                { "Id", "Order ID" },
                { "StudentName", "Student Name" },
                { "OrderTime", "Order Date" },
                { "Status", "Status" },
                { "TotalPrice", "Total Amount" },
                { "PaymentIntentId", "Payment ID" },
                { "Classes", "Classes" }
            };

            var model = new ListVM<OrderVM>
            {
                Items = orderVMs,
                PagingInfo = new PagingVM
                {
                    CurrentPage = orderParams.Page,
                    TotalItems = totalOrders.Count(),
                    ItemsPerPage = PageSize
                },
                SearchValue = new SearchbarVM
                {
                    Controller = "OrdersManagement",
                    Action = "Index",
                    SearchText = search
                }
            };

            return View(model);
        }
    }
}