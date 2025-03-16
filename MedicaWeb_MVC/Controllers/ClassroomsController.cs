using AutoMapper;
using Core.Constants;
using Core.Entities;
using Core.Interfaces.Repos;
using Core.Interfaces.Services;
using Core.Specifications.Classes;
using Core.Specifications.Courses;
using Infrastructure.Services;
using MedicaWeb_MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Stripe.Climate;

namespace MedicaWeb_MVC.Controllers
{
    public class ClassroomsController : Controller
    {
        private readonly ILogger<ClassroomsController> _logger;
        private readonly IClassService _classService;
        private readonly ICourseService _courseService;
        private readonly ILecturerService _lecturerService;
        private readonly IAccountService _accountService;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public ClassroomsController(
            ILogger<ClassroomsController> logger,
            ICourseService courseService, IClassService classService, IMapper mapper,
            ILecturerService lecturerService, IAccountService accountService, IOrderService orderService)
        {
            _logger = logger;
            _classService = classService;
            _courseService = courseService;
            _mapper = mapper;
            _lecturerService = lecturerService;
            _accountService = accountService;
            _orderService = orderService;
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
            if (isUpdate)
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
        [Route("Classrooms/MyClass")]
        [Authorize]
        public async Task<IActionResult> MyClass([FromQuery] ClassParams param)
        {
            var user = await _accountService.GetUserByClaimsAsync(User);
            if (user == null)
            {
                return NotFound();
            }
            IEnumerable<Classroom> classes;
            string url;
            // Retrieve student's learning
            if (User.IsInRole(AppCts.Roles.Student))
            {
                classes = await _orderService.GetMyLearningByStudentId(user.Id, param.ClassroomStatus);
                url = "~/Views/Classrooms/MyLearning.cshtml";
            }
            // retrieve lecturer's classes
            else
            {
                param.LecturerId = user.Id;
                var spec = new ClassSpecification(param);
                classes = await _classService.GetClassesAsync(spec);
                url = "~/Views/Classrooms/MyClass.cshtml";
            }

            var model = new MyLearningVM
            {
                Classes = _mapper.Map<IEnumerable<ClassVM>>(classes),
                SelectedStatus = param.ClassroomStatus
            };
            return View(url, model);
        }
        [HttpGet]
        [Authorize(Roles = AppCts.Roles.Employee)]
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

            return RedirectToAction(nameof(Index), new { CourseId = courseId });
        }
        [HttpPost]
        [Authorize(Roles = AppCts.Roles.Employee)]
        public async Task<IActionResult> Upsert(ClassUpsertVM classUpsertVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Classroom classroom;
                    if (classUpsertVM.Id == 0)
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
                    TempData["error"] = "Failed to update this class due to an unexpected error.";
                    Console.WriteLine(ex);
                }
                return RedirectToAction(nameof(Index), new { CourseId = classUpsertVM.CourseId });
            }
            return RedirectToAction(nameof(Index), new { CourseId = classUpsertVM.CourseId });
        }

        [Authorize(Roles = AppCts.Roles.Employee)]
        public async Task<IActionResult> Delete(int id, int courseId)
        {
            try
            {
                await _classService.DeleteClassAsync(id);
                TempData["success"] = "Successfully delete this class";
            }
            catch (KeyNotFoundException e)
            {
                TempData["error"] = "Class not found. Please check again.";
                Console.WriteLine(e);
            }
            catch (InvalidOperationException e)
            {
                TempData["error"] = "Unable to delete this class due to invalid state.";
                Console.WriteLine(e);

            }
            catch (Exception e)
            {
                TempData["error"] = "Failed to delete this class due to an unexpected error.";
                Console.WriteLine(e);

            }
            return RedirectToAction(nameof(Index), new { courseId = courseId });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var specification = new ClassSpecification(id);
            var classroom = await _classService.GetClassAsync(specification);

            if (classroom == null)
            {
                return NotFound();
            }
            var classVM = _mapper.Map<ClassVM>(classroom);
            //classVM.Students = (IEnumerable<StudentVM>)classroom.OrderDetails.Select(od => od.Order.Student).ToList();

            return View(classVM);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }

}