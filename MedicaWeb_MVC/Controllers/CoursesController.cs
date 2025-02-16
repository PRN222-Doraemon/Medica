using Core.Entities;
using Core.Interfaces.Repos;
using Core.Interfaces.Services;
using Core.Specifications.Courses;
using MedicaWeb_MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol.Core.Types;

namespace MedicaWeb_MVC.Controllers
{
    public class CoursesController : Controller
    {
        private ICourseService _courseService;
        private IGenericRepository<Category> _categoryRepo;
        public CoursesController(ICourseService courseService, IGenericRepository<Category> categoryRepo)
        {
            _courseService = courseService;
            _categoryRepo = categoryRepo;
        }
        public async Task<IActionResult> Index(CourseParams courseParams)
        {
            ViewData["Categories"] = new SelectList(await _categoryRepo.ListAllAsync(), "Id", "Name");
            ViewData["Status"] = new SelectList(new List<string> { "Active", "Inactive"});
            var spec = new CourseSpecification(courseParams);
            var courses = await _courseService.GetCoursesAsync(spec);

            var model = new ListVM<Course>
            {
                Items = courses,
                PagingInfo = new PagingVM { CurrentPage = courseParams.PageIndex, TotalItems = courses.Count() }
            };
            return View(model);
        }
    }
}
