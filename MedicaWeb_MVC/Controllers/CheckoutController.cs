using AutoMapper;
using Core.Entities;
using Core.Interfaces.Services;
using MedicaWeb_MVC.ViewModels;
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
        private readonly IOrderService _orderService;

        // ==============================
        // === Constructors
        // ==============================
        public CheckoutController(ICartService cartService, IAccountService accountService, IPaymentService paymentService, IMapper mapper, IOrderService orderService)
        {
            _cartService = cartService;
            _accountService = accountService;
            _paymentService = paymentService;
            _mapper = mapper;
            _orderService = orderService;
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
            return View(_mapper.Map<IEnumerable<CartItem>, IEnumerable<CartItemVM>>(cartItems));
        }

        // Return the session id for Checkout UI
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
            var baseUrl = $"{Request.Scheme}://${Request.Host}";
            var successUrl = $"{baseUrl}/Checkout/Success?session_id={{CHECKOUT_SESSION_ID}}";
            var cancelUrl = $"{baseUrl}/Checkout/Cancel";
            var sessionId = await _paymentService.CreateCheckoutSessionAsync(
                cartItems,
                user.Id,
                successUrl,
                cancelUrl);
            return Json(new { sessionId = sessionId });
        }
    }
}
