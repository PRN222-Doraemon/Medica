using AutoMapper;
using Core.Constants;
using Core.Entities;
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

        public async Task<IActionResult> Index(ClassroomStatus? selectedStatus)
        {
            var user = await _accountService.GetUserByClaimsAsync(User);
            var classes = await _orderService.GetMyLearningByStudentId(user.Id, selectedStatus);
            var myLearning = new MyLearningVM
            {
                Classes = _mapper.Map<IEnumerable<ClassVM>>(classes),
                SelectedStatus = selectedStatus
            };
            return View(myLearning);
        }
    }
}
