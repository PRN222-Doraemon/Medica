using Core.Entities;
using Core.Interfaces.Repos;
using Infrastructure.Specifications.ClassSpecifications;
using MedicaWeb_MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Index()
        {
            var specification = new ClassroomWithCourseSpecification();
            var classrooms = await _unitOfWork.Repository<Classroom>().ListAsync(specification);
            var viewModel = new LecturerClassroomVM
            {
                Classrooms = classrooms.ToList(),
                Categories = classrooms.Select(c => c.Course.Category).Distinct().ToList()
            };
            return View(viewModel);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ClassroomDetails(int id)
        {
            var classroom = await _unitOfWork.Repository<Classroom>().GetByIdAsync(id);
            return View(classroom);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }

}