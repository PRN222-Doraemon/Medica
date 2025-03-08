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
using Newtonsoft.Json;

namespace MedicaWeb_MVC.Controllers
{
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

            // save data for update popup
            bool isUpdate = TempData["IsUpdate"] != null && (bool)TempData["IsUpdate"];
            ClassUpsertVM classroom = TempData["Classroom"] != null
                ? JsonConvert.DeserializeObject<ClassUpsertVM>(TempData["Classroom"].ToString())
                : null;

            ViewData["IsUpdate"] = isUpdate;
            ViewData["Classroom"] = classroom;
            if(isUpdate)
            {
                @ViewData["Title"] = "Update Class";
            }
            else
            {
                @ViewData["Title"] = "Create Class";
            }

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

        [HttpGet]
        public async Task<IActionResult> Upsert(int? id, int courseId)
        {
            ViewData["LecturerIds"] = new SelectList(
                await _lecturerService.GetLecturersAsync(),
                "Id",
                "FullName");
            ViewData["IsUpdate"] = false;

            // update class
            if (id != null)
            {
                var classroom = await _classService.GetClassByIdAsync(id.Value);
                if (classroom == null)
                {
                    return NotFound();
                }
                TempData["Classroom"] = JsonConvert.SerializeObject(_mapper.Map<ClassUpsertVM>(classroom));
                TempData["IsUpdate"] = true;
            }

            return RedirectToAction(nameof(Index), new {CourseId = courseId});
        }
        [HttpPost]
        public async Task<IActionResult> Upsert(ClassUpsertVM classUpsertVM)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    Classroom classroom;
                    if(classUpsertVM.Id == 0)
                    {
                        classroom = _mapper.Map<Classroom>(classUpsertVM);
                        await _classService.CreateClassAsync(classroom);
                        TempData["success"] = "Successfully create a new class";
                    }
                    else
                    {
                        classroom = await _classService.GetClassByIdAsync(classUpsertVM.Id);
                        _mapper.Map(classUpsertVM, classroom);
                        await _classService.UpdateClassAsync(classroom);
                        TempData["success"] = "Successfully update class";
                    }
                }
                catch (Exception ex)
                {
                    TempData["error"] = ex.ToString();
                }
                return RedirectToAction(nameof(Index), new { CourseId = classUpsertVM.CourseId});
            }
            return RedirectToAction(nameof(Index), new { CourseId = classUpsertVM.CourseId });
        }

        public async Task<IActionResult> Delete(int id, int courseId)
        {
            try
            {
                await _classService.DeleteClassAsync(id);
                TempData["success"] = "Successfully delete this class";
            }
            catch(Exception e)
            {
                TempData["error"] = "Fail to delete this class";
            }
            return RedirectToAction(nameof(Index), new { courseId = courseId });
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