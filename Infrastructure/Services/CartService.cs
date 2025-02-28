using Core.DTOs;
using Core.Entities;
using Core.Interfaces.Repos;
using Core.Interfaces.Services;
using Core.Specifications.Classes;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CartService : ICartService
    {
        // ==============================
        // === Props & Fields
        // ==============================

        private readonly IDistributedCache _cache;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClassService _classService;

        // ==============================
        // === Constructors
        // ==============================

        public CartService(IDistributedCache cache, IUnitOfWork unitOfWork, IClassService classService)
        {
            _cache = cache;
            _unitOfWork = unitOfWork;
            _classService = classService;
        }

        // ==============================
        // === Methods
        // ==============================

        public async Task AddOrUpdateToCartAsync(int classRoomId, int userId)
        {
            var spec = new ClassSpecification(classRoomId);
            var classRoom = await _classService.GetClassAsync(spec);

            if (classRoom == null)
            {
                throw new KeyNotFoundException($"Classroom with ID {classRoomId} not found.");
            }

            // Retrieve the cartItems from the Redis
            var cartKey = $"cart_{userId}";
            var cartItems = await GetCartItemsAsync(userId);
            if (cartItems.Any(ci => ci.ClassRoomId == classRoomId))
            {
                throw new Exception($"Already add this course. Please add another course.");
            }

            // Add new CartItem and store back to the Redis
            cartItems.Add(new CartItem()
            {
                ClassRoomId = classRoomId,
                CourseId = classRoom.CourseId,
                CouresName = classRoom.Course.Name,
                Price = classRoom.Course.Price,
            });

            await _cache.SetStringAsync(cartKey, JsonSerializer.Serialize(cartItems), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24) // Valid only 24 hours
            });
        }

        public async Task DeleteCartAsync(int userId)
        {
            await _cache.RemoveAsync($"cart_{userId}");
        }

        public async Task<List<CartItem>> GetCartItemsAsync(int userId)
        {
            var cartKey = $"cart_{userId}";
            var cartList = await _cache.GetStringAsync(cartKey);
            return string.IsNullOrEmpty(cartList)
                ? []
                : JsonSerializer.Deserialize<List<CartItem>>(cartList) ?? [];
        }
    }
}
