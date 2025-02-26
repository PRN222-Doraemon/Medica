using Core.Entities;
using Core.Interfaces.Repos;
using Microsoft.AspNetCore.Mvc;
using Core.ViewModels;
using Core.Specifications.Classes;
using Core.Specifications.Courses;
using Microsoft.AspNetCore.Mvc.Rendering;
using Core.Interfaces.Services;
using MedicaWeb_MVC.ViewModels;
using AutoMapper;

namespace MedicaWeb_MVC.Controllers
{
    [Route("[controller]")]
    public class ClassroomsController : Controller
    {
        private readonly ILogger<ClassroomsController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClassService _classService;
        private readonly IMapper _mapper;

        public ClassroomsController(
            ILogger<ClassroomsController> logger,
            IUnitOfWork unitOfWork, IClassService classService, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _classService = classService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync([FromQuery] ClassParams classParams)
        {
            if (classParams.CourseId != null)
            {
                var courseSpec = new CourseSpecification(classParams.CourseId.Value);
                var course = await _unitOfWork.Repository<Course>().GetEntityWithSpec(courseSpec);
                ViewData["Course"] = _mapper.Map<CourseVM>(course);
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

        [HttpGet("{id}")]
        public async Task<IActionResult> ClassroomDetails(int id)
        {
            var classroom = await _unitOfWork.Repository<Classroom>().GetByIdAsync(id);
            return View(classroom);
        }

        //[HttpGet("search")]
        //public async Task<IActionResult> Search(LecturerClassroomVM lecturerClassroomVM)
        //{
        //    var specification = new classroom(lecturerClassroomVM);
        //    var classrooms = await _unitOfWork.Repository<Classroom>().ListAsync(specification);
        //    var categories = await _unitOfWork.Repository<Category>().ListAllAsync();

        //    var viewModel = new LecturerClassroomVM
        //    {
        //        Classrooms = classrooms.ToList(),
        //        Categories = categories.ToList()
        //    };

        //    return View("Index", viewModel);
        //}

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
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