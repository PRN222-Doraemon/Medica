using AutoMapper;
using Core.Entities;
using Core.Interfaces.Services;
using MedicaWeb_MVC.ViewModels.Courses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace MedicaWeb_MVC.Controllers
{
    public class CommentsController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICommentService _commentService;

        public CommentsController(IMapper mapper, ICommentService commentService)
        {
            _mapper = mapper;
            _commentService = commentService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Upsert(CommentCreateVM commentCreateVM)
        {
            var comment = _mapper.Map<Comment>(commentCreateVM);
            await _commentService.AddComment(comment);
            TempData["success"] = "Successfully added a new question";
            return RedirectToAction("Details", "Classrooms" ,commentCreateVM.ClassID);
        }
    }
}
