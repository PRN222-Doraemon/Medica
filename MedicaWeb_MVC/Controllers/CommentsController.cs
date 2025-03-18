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
        private readonly IAccountService _userService;

        public CommentsController(IMapper mapper, ICommentService commentService, IAccountService userService)
        {
            _mapper = mapper;
            _commentService = commentService;
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Upsert(CommentCreateVM commentCreateVM)
        {
            var comment = _mapper.Map<Comment>(commentCreateVM);
            var user = await _userService.GetUserByClaimsAsync(User);
            comment.UserID = user.Id;
            await _commentService.AddComment(comment);
            TempData["success"] = "Successfully added a comment";
            return RedirectToAction("Details", "Classrooms", new { id = commentCreateVM.ClassID });
        }
    }
}
