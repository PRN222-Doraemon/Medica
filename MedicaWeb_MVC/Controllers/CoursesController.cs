using AutoMapper;
using Core.Constants;
using Core.Entities;
using Core.Interfaces.Repos;
using Core.Interfaces.Services;
using Core.Specifications.Courses;
using MedicaWeb_MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System.Net.WebSockets;

namespace MedicaWeb_MVC.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IGenericRepository<Category> _categoryRepo;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public CoursesController(ICourseService courseService, IGenericRepository<Category> categoryRepo, 
            IMapper mapper, IUserService userService, ICloudinaryService cloudinaryService)
        {
            _courseService = courseService;
            _categoryRepo = categoryRepo;
            _mapper = mapper;
            _userService = userService;
            _cloudinaryService = cloudinaryService;
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
            var countSpec = new CourseSpecification(courseParams, false);
            var courses = await _courseService.GetCoursesAsync(spec);
            var totalCourses = (await _courseService.GetCoursesAsync(countSpec)).Count();

            var model = new ListVM<CourseVM>
            {
                Items = _mapper.Map<IEnumerable<CourseVM>>(courses),
                PagingInfo = new PagingVM { CurrentPage = courseParams.Page, TotalItems = totalCourses },
                SearchValue = new SearchbarVM { Controller = "Courses", Action = "Index", SearchText = courseParams.Search}
            };
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var course = await _courseService.GetCourseByIdAsync(id);
            return View(_mapper.Map<CourseVM>(course));
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            ViewData["Categories"] = new SelectList(await _categoryRepo.ListAllAsync(), "Id", "Name");
            ViewData["ResourceTypes"] = new SelectList(new List<string> { ResourceType.Slide.ToString(), ResourceType.Video.ToString()});

            // create a new course
            if (id == null)
            {
                ViewData["Title"] = "Create Course";
                return View(new CourseCreateVM());
            }

            // Update course
            ViewData["Title"] = "Update Course";
            var course = await _courseService.GetCourseByIdAsync(id.Value);
            if (course == null)
            {
                return NotFound();
            }
            return View(_mapper.Map<CourseCreateVM>(course));
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(CourseCreateVM courseVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Course course;

                    // upload image to Cloudinary and save image url
                    if(courseVM.ImageFile != null)
                    {
                        courseVM.ImgUrl = await _cloudinaryService.UploadAsync(courseVM.ImageFile);                       
                    }
                    // up load file
                    foreach(var chapter in courseVM.CourseChapters)
                    {
                        foreach(var resource in chapter.Resources)
                        {
                            if (resource.File != null)
                            {
                                resource.FileUrl = await _cloudinaryService.UploadAsync(resource.File);
                            }
                        }
                    }

                    // Add new course
                    if (courseVM.Id == 0)
                    {
                        course = _mapper.Map<Course>(courseVM);
                        // created by user ID value is just for testing, it will be updated later
                        course.CreatedByUserID = 1;
                        await _courseService.CreateCourseAsync(course);
                        TempData["success"] = "Successfully created a new course!";
                    }
                    // Update course
                    else
                    {
                        course = await _courseService.GetCourseByIdAsync(courseVM.Id);
                        if (course == null)
                        {
                            return NotFound();
                        }
                        _mapper.Map(courseVM, course);
                        await _courseService.UpdateCourseAsync(course);
                        TempData["success"] = "Successfully updated a new course!";
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["error"] = ex.InnerException?.Message ?? ex.Message;
                }
            }          
            ViewData["Categories"] = new SelectList(await _categoryRepo.ListAllAsync(), "Id", "Name");
            ViewData["ResourceTypes"] = new SelectList(new List<string> { ResourceType.Slide.ToString(), ResourceType.Video.ToString() }
            );
            return View(courseVM);
        }
    }
}
