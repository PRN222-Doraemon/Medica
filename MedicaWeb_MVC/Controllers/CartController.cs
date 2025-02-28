using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces.Services;
using MedicaWeb_MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MedicaWeb_MVC.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        // ==============================
        // === Fields & Props
        // ==============================

        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        // ==============================
        // === Constructors
        // ==============================

        public CartController(IOrderService orderService, ICartService cartService, IAccountService accountService, IMapper mapper)
        {
            _orderService = orderService;
            _cartService = cartService;
            _accountService = accountService;
            _mapper = mapper;
        }

        // ==============================
        // === Methods
        // ==============================

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _accountService.GetUserByClaimsAsync(User);
            if (user == null) return Unauthorized();

            var cartItems = await _cartService.GetCartItemsAsync(user.Id);

            return View(_mapper.Map<IEnumerable<CartItem>, IEnumerable<CartItemVM>>(cartItems));
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto addToCartDto)
        {
            var user = await _accountService.GetUserByClaimsAsync(User);
            if (user == null) return Json(new ResponseJson() { IsSuccess = false, Message = $"User not found. Please try again." });

            // Adding classroom to cart
            try
            {
                await _cartService.AddOrUpdateToCartAsync(addToCartDto.ClassRoomId, user.Id);
                TempData["success"] = "Classroom added successfully.";
                return Json(new ResponseJson() { IsSuccess = true, Message = "Classroom added successfully." });
            }
            catch (Exception ex)
            {
                TempData["error"] = $"Failed to add: {ex.Message}";
                return Json(new ResponseJson() { IsSuccess = false, Message = $"Failed to add: {ex.Message}" });
            }
        }
    }
}
