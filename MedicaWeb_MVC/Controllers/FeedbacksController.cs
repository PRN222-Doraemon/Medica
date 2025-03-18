using AutoMapper;
using Core.Entities;
using Core.Interfaces.Services;
using MedicaWeb_MVC.ViewModels.Courses;
using Microsoft.AspNetCore.Mvc;

namespace MedicaWeb_MVC.Controllers
{
    public class FeedbacksController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IFeedbackService _feedbackService;
        private readonly IMapper _mapper;

        public FeedbacksController(IAccountService accountService, IFeedbackService feedbackService, IMapper mapper)
        {
            _accountService = accountService;
            _feedbackService = feedbackService;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FeedbackUpsertVM feedbackVM)
        {
            try
            {
                var user = await _accountService.GetUserByClaimsAsync(HttpContext.User);
                var feedback = _mapper.Map<Feedback>(feedbackVM);
                feedback.StudentId = user.Id;
                await _feedbackService.CreateFeedBack(feedback);
                TempData["success"] = "Successfully create a feedback";
            }
            catch (Exception ex)
            {
                TempData["error"] = "Fail to create a feedback";
            }
            return RedirectToAction("Details", "Courses", new { id = feedbackVM.CourseId });
        }

        [HttpPost]
        public async Task<IActionResult> Update(FeedbackUpsertVM feedbackVM)
        {
            try
            {
                var user = await _accountService.GetUserByClaimsAsync(HttpContext.User);
                var feedback = await _feedbackService.GetFeedbackByIdAsync(feedbackVM.Id);
                if (feedback == null)
                {
                    return NotFound();
                }
                feedback = _mapper.Map<Feedback>(feedbackVM);
                feedback.StudentId = user.Id;
                await _feedbackService.UpdateFeedBack(feedback);
                TempData["success"] = "Successfully create a feedback";
            }
            catch (Exception ex)
            {
                TempData["error"] = "Fail to create a feedback";
            }
            return RedirectToAction("Details", "Courses", feedbackVM.CourseId);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var feedback = await _feedbackService.GetFeedbackByIdAsync(id);
            if (feedback == null)
            {
                return NotFound();
            }
            await _feedbackService.DeleteFeedback(feedback);
            TempData["success"] = "Successfully delete";
            return RedirectToAction("Details", "Courses", feedback.CourseId);

        }
    }
}
