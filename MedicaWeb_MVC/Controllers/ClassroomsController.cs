using AutoMapper;
using Core.Entities;
using Core.Interfaces.Repos;
using Core.Interfaces.Services;
using Core.Specifications.Classes;
using Core.Specifications.Courses;
using Infrastructure.Services;
using MedicaWeb_MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MedicaWeb_MVC.Controllers
{
    [Route("[controller]")]
    public class ClassroomsController : Controller
    {
        private readonly ILogger<ClassroomsController> _logger;
        private readonly IClassService _classService;
        private readonly ICourseService _courseService;
        private readonly ILecturerService _lecturerService;
        private readonly IMapper _mapper;

        public ClassroomsController(
            ILogger<ClassroomsController> logger,
            ICourseService courseService, IClassService classService, IMapper mapper, ILecturerService lecturerService)
        {
            _logger = logger;
            _classService = classService;
            _courseService = courseService;
            _mapper = mapper;
            _lecturerService = lecturerService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] ClassParams classParams)
        {
            ViewData["LecturerIds"] = new SelectList(
                await _lecturerService.GetLecturersAsync(),
                "Id",
                "FullName");
            if (classParams.CourseId != null)
            {
                var course = await _courseService.GetCourseByIdAsync(classParams.CourseId.Value);
                ViewData["Course"] = _mapper.Map<CourseVM>(course);
                ViewData["Lecturers"] = _mapper.Map<CourseVM>(course).Classrooms
                .Select(c => c.Lecturer).GroupBy(l => l.Id).Select(g => g.First()).ToList();
            }
            var spec = new ClassSpecification(classParams);
            var countSpec = new ClassSpecification(classParams, false);
            var classes = await _classService.GetClassesAsync(spec);
            var totalClasses = (await _classService.GetClassesAsync(countSpec)).Count();

            

            var model = new ListVM<ClassVM>
            {
                Items = _mapper.Map<IEnumerable<ClassVM>>(classes),
                PagingInfo = new PagingVM { CurrentPage = classParams.Page, TotalItems = totalClasses },
                SearchValue = new SearchbarVM { Controller = "LecturerClassroom", Action = "Index", SearchText = classParams.Search },
                ClassroomStatus = classParams.ClassroomStatus
            };
            return View(model);

        }
        

        [HttpPost]
        public async Task<IActionResult> Create(ClassUpsertVM classUpsertVM)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    var classroom = _mapper.Map<Classroom>(classUpsertVM);
                    await _classService.CreateClassAsync(classroom);
                    TempData["success"] = "Successfully create a new class";
                }
                catch (Exception ex)
                {
                    TempData["error"] = ex.ToString();
                }
                return RedirectToAction(nameof(Index), new { CourseId = classUpsertVM.CourseId});
            }
            return RedirectToAction(nameof(Index), new { CourseId = classUpsertVM.CourseId });
        }

        //[HttpGet("details/{id}")]
        //public async Task<IActionResult> Details(int id)
        //{
        //    var specification = new ClassroomDetailsSpecification(id);
        //    var classroom = await _unitOfWork.Repository<Classroom>().GetEntityWithSpec(specification);

        //    if (classroom == null)
        //    {
        //        return NotFound();
        //    }

        //    var viewModel = new ClassDetailsVM
        //    {
        //        Classroom = classroom,
        //        Course = classroom.Course
        //    };

        //    return View(viewModel);
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }

}