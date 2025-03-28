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
        private readonly ILecturerService _lecturerService;
        private readonly IStudentService _studentService;
        private readonly IFeedbackService _feedbackService;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;

        // =========================
        // === Constructors
        // =========================
        public HomeController(ICourseService courseService, IMapper mapper, IAccountService accountService,
        ILecturerService lecturerService, IStudentService studentService, IFeedbackService feedbackService)
        {
            _courseService = courseService;
            _mapper = mapper;
            _accountService = accountService;
            _lecturerService = lecturerService;
            _studentService = studentService;
            _feedbackService = feedbackService;
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
            var totalCourses = (await _courseService.GetAllCoursesAsync()).Count();
            var totalLecturers = (await _lecturerService.GetLecturersAsync()).Count();
            var totalStudents = (await _studentService.GetAllStudentsAsync()).Count();
            var totalFeedback = (await _feedbackService.GetAllFeedbacks()).Count();

            TempData["totalCourses"] = totalCourses;
            TempData["totalLecturers"] = totalLecturers;
            TempData["totalStudents"] = totalStudents;
            TempData["totalFeedbacks"] = totalFeedback;

            var spec = new TopCoursesByFeedbacksSpecification(3);
            var courses = await _courseService.GetCoursesAsync(spec);
            return View(_mapper.Map<IEnumerable<Course>, IEnumerable<CourseVM>>(courses));
        }
    }
}
