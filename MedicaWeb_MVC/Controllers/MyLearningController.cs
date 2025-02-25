using AutoMapper;
using Core.Constants;
using Core.Interfaces.Services;
using MedicaWeb_MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicaWeb_MVC.Controllers
{
    [Authorize(Roles = AppCts.Roles.Student)]
    public class MyLearningController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        public MyLearningController(IOrderService orderService, IAccountService accountService, IMapper mapper)
        {
            _orderService = orderService;
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(SelectedFilter? selectedFilter)
        {
            var user = await _accountService.GetUserByClaimsAsync(User);
            var orders = await _orderService.GetOrdersByStudentId(user.Id);
            var orderDetails = orders.SelectMany(o => o.OrderDetails).ToList(); 
            var classes = orderDetails.Select(od => od.Classroom).ToList();
            // filter by status
            if (selectedFilter != null)
            {
                switch (selectedFilter)
                {
                    case SelectedFilter.UpComing:
                        classes = classes.Where(c => c.StartDate > DateOnly.FromDateTime(DateTime.UtcNow)).ToList();
                        break;
                    case SelectedFilter.Completed:
                        classes = classes.Where(c => c.EndDate < DateOnly.FromDateTime(DateTime.UtcNow)).ToList();
                        break;
                    case SelectedFilter.InProgress:
                        classes = classes.Where(c => c.StartDate <= DateOnly.FromDateTime(DateTime.UtcNow)
                                  && c.EndDate > DateOnly.FromDateTime(DateTime.UtcNow)).ToList();
                        break;
                }
            }
            var myLearning = new MyLearningVM
            {
                Classes = _mapper.Map<IEnumerable<ClassVM>>(classes),
                SelectedFilter = selectedFilter ?? SelectedFilter.All
            };
            return View(myLearning);    
        }
    }
}
