using AutoMapper;
using Core.Entities;
using Core.Interfaces.Repos;
using Core.Interfaces.Services;
using Core.Specifications.Courses;
using MedicaWeb_MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol.Core.Types;
using System.Net.WebSockets;

namespace MedicaWeb_MVC.Controllers
{
    public class CoursesController : Controller
    {
        private ICourseService _courseService;
        private IGenericRepository<Category> _categoryRepo;
        private IMapper _mapper;
        public CoursesController(ICourseService courseService, IGenericRepository<Category> categoryRepo, IMapper mapper)
        {
            _courseService = courseService;
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index([FromQuery]CourseParams courseParams)
        {
            ViewData["Categories"] = new SelectList(
                await _categoryRepo.ListAllAsync(), 
                "Id", 
                "Name", 
                courseParams.CategoryID);

            ViewData["Status"] = new SelectList(
                new List<string> { CourseStatus.Active.ToString(), CourseStatus.Inactive.ToString() }, 
                selectedValue: courseParams.Status?.ToString() ?? CourseStatus.Active.ToString());

            var spec = new CourseSpecification(courseParams);
            var courses = await _courseService.GetCoursesAsync(spec);

            var model = new ListVM<CourseVM>
            {
                Items = _mapper.Map<IEnumerable<CourseVM>>(courses),
                PagingInfo = new PagingVM { CurrentPage = courseParams.PageIndex, TotalItems = courses.Count() },
                SearchValue = new SearchbarVM { Controller = "Courses", Action = "Index", SearchText = courseParams.Search}
            };
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            return View(_mapper.Map<CourseVM>(course));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            return View(_mapper.Map<CourseVM>(course));
        }

        [HttpPost]
        public async Task<IActionResult> Update(CourseCreateVM courseVM)
        {
            var course = _mapper.Map<Course>(courseVM);
            await _courseService.UpdateCourseAsync(course);
            course = await _courseService.GetCourseByIdAsync(course.Id);
            return View(_mapper.Map<CourseVM>(course));
        }

        public async Task<IActionResult> CreateAsync()
        {
            ViewData["Categories"] = new SelectList(await _categoryRepo.ListAllAsync(), "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateVM courseVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var course = _mapper.Map<Course>(courseVM);
                    await _courseService.CreateCourseAsync(course);
                    TempData["success"] = "Successfully created a new course!";
                    return RedirectToAction(nameof(Index));                  
                }
                catch (Exception ex)
                {
                    TempData["error"] = ex.Message;
                }
            }
            ViewData["Categories"] = new SelectList(await _categoryRepo.ListAllAsync(), "Id", "Name");
            return View(courseVM);
        }
    }
}
