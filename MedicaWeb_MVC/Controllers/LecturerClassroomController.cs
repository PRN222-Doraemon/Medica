using Core.Entities;
using Core.Interfaces.Repos;
using Microsoft.AspNetCore.Mvc;
using Core.ViewModels;

namespace MedicaWeb_MVC.Controllers
{
    [Route("[controller]")]
    public class LecturerClassroomController : Controller
    {
        private readonly ILogger<LecturerClassroomController> _logger;
        private readonly IUnitOfWork _unitOfWork;


        public LecturerClassroomController(
            ILogger<LecturerClassroomController> logger,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction("Search");

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