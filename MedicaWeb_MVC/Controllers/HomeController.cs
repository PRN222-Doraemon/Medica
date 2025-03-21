using AutoMapper;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Specifications.Courses;
using MedicaWeb_MVC.ViewModels.Courses;
using Microsoft.AspNetCore.Mvc;

namespace MedicaWeb_MVC.Controllers
{
    public class HomeController : Controller
    {
        // =========================
        // === Fields & Props
        // =========================

        private readonly ICourseService _courseService;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        // =========================
        // === Constructors
        // =========================
        public HomeController(ICourseService courseService, IMapper mapper, IAccountService accountService)
        {
            _courseService = courseService;
            _mapper = mapper;
            _accountService = accountService;
        }

        // =========================
        // === Methods
        // =========================

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // if (_accountService.IsSignedIn(User))
            // {
            //     return RedirectToAction("Index", "Courses");
            // }
            var spec = new TopCoursesByFeedbacksSpecification(3);
            var courses = await _courseService.GetCoursesAsync(spec);
            return View(_mapper.Map<IEnumerable<Course>, IEnumerable<CourseVM>>(courses));
        }
    }
}
