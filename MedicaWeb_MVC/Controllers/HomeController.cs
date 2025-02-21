using AutoMapper;
using Core.Entities;
using Core.Interfaces.Repos;
using Core.Interfaces.Services;
using Core.Specifications.Courses;
using MedicaWeb_MVC.Models;
using MedicaWeb_MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace MedicaWeb_MVC.Controllers
{
    public class HomeController : Controller
    {
        // =========================
        // === Fields & Props
        // =========================

        private readonly ICourseService _courseService;
        private readonly IMapper _mapper;

        // =========================
        // === Constructors
        // =========================
        public HomeController(ICourseService courseService, IMapper mapper)
        {
            _courseService = courseService;
            _mapper = mapper;
        }

        // =========================
        // === Methods
        // =========================

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var spec = new TopCoursesByFeedbacksSpecification(3);
            var courses = await _courseService.GetCoursesAsync(spec);
            return View(_mapper.Map<IEnumerable<Course>, IEnumerable<CourseVM>>(courses));
        }
    }
}
