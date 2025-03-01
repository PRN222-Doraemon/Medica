using AutoMapper;
using Core.Entities;
using Core.Interfaces.Repos;
using Core.Interfaces.Services;
using Core.Specifications.Classes;
using Core.Specifications.Courses;
using MedicaWeb_MVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MedicaWeb_MVC.Controllers
{
    public class CheckoutController : Controller
    {
        // ==============================
        // === Fields & Props
        // ==============================

        private readonly ICartService _cartService;
        private readonly IAccountService _accountService;
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IOrderService _orderService;
        private readonly IUnitOfWork _unitOfWork;

        // ==============================
        // === Constructors
        // ==============================

        public CheckoutController(ICartService cartService, IAccountService accountService, IPaymentService paymentService, IMapper mapper, IConfiguration configuration, IOrderService orderService, IUnitOfWork unitOfWork)
        {
            _cartService = cartService;
            _accountService = accountService;
            _paymentService = paymentService;
            _mapper = mapper;
            _configuration = configuration;
            _orderService = orderService;
            _unitOfWork = unitOfWork;
        }

        // ==============================
        // === Methods
        // ==============================

        [HttpGet]
        public async Task<IActionResult> OrderSummary()
        {
            var user = await _accountService.GetUserByClaimsAsync(User);
            if (user == null) return Unauthorized();

            // If cart dont have anything than no further checkout
            var cartItems = await _cartService.GetCartItemsAsync(user.Id);
            if (cartItems.Count == 0)
            {
                TempData["error"] = "Your cart is empty.";
                return RedirectToAction("Index", "Cart");
            }

            var cartItemsVM = _mapper.Map<IEnumerable<CartItem>, IEnumerable<CartItemVM>>(cartItems);

            foreach (var item in cartItemsVM)
            {
                var course = await _unitOfWork.Repository<Course>().GetEntityWithSpec(new CourseSpecification(item.CourseId));
                var classroom = await _unitOfWork.Repository<Classroom>().GetEntityWithSpec(new ClassSpecification(item.ClassRoomId));

                if (course != null)
                {
                    item.Duration = course.Duration;
                    item.Description = course.Description;
                    item.ImageUrl = course.ImgUrl;
                }

                if (classroom != null)
                {
                    item.InstructorName = classroom.Lecturer.FullName;
                    item.StartDate = classroom.StartDate;
                    item.Mode = classroom.Mode;
                }
            }

            return View(cartItemsVM);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            var user = await _accountService.GetUserByClaimsAsync(User);
            if (user == null) return Unauthorized();

            var cartItems = await _cartService.GetCartItemsAsync(user.Id);
            if (cartItems.Count == 0)
            {
                TempData["error"] = "Your cart is empty.";
                return RedirectToAction("Index", "Cart");
            }

            // Create Checkout Session with return session id back to view
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var successUrl = $"{baseUrl}/{_configuration["Stripe:SuccessUrl"]}";
            var cancelUrl = $"{baseUrl}/{_configuration["Stripe:CancelUrl"]}";

            var sessionId = await _paymentService.CreateCheckoutSessionAsync(
                cartItems,
                user.Id,
                successUrl,
                cancelUrl);

            return Json(new { sessionId });
        }

        [HttpGet]
        public async Task<IActionResult> Success()
        {
            TempData["success"] = "Payment received. Your order is being processed.";
            return View();
        }

        [HttpGet]
        public IActionResult Cancel()
        {
            TempData["error"] = "Checkout was cancelled.";
            return RedirectToAction(nameof(OrderSummary));
        }
    }
}
