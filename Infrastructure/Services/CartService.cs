using Core.Constants;
using Core.Entities;
using Core.Interfaces.Services;
using Core.Specifications.Classes;
using StackExchange.Redis;
using System.Globalization;
using System.Text.Json;

namespace Infrastructure.Services
{
    public class CartService : ICartService
    {
        // ==============================
        // === Props & Fields
        // ==============================

        private readonly IDatabase _redisDb;
        private readonly IClassService _classService;

        // ==============================
        // === Constructors
        // ==============================

        public CartService(IConnectionMultiplexer connectionMultiplexer, IClassService classService)
        {
            _redisDb = connectionMultiplexer.GetDatabase(AppCts.RedisDatabase.Cart.Database); // Database 0 for Redis
            _classService = classService;
        }

        // ==============================
        // === Methods
        // ==============================

        /// <summary>
        /// Add or Update to the CartDatabase
        /// </summary>
        /// <param name="classRoomId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task AddOrUpdateToCartAsync(int classRoomId, int userId)
        {
            var spec = new ClassSpecification(classRoomId);
            var classRoom = await _classService.GetClassAsync(spec);

            if (classRoom == null)
            {
                throw new KeyNotFoundException($"Classroom with ID {classRoomId} not found.");
            }

            // Retrieve the cartItems from the Redis
            var key = string.Format(AppCts.RedisDatabase.Cart.KeyTemplate, userId);
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
                CourseName = classRoom.Course.Name,
                ImageUrl = classRoom.Course.ImgUrl ?? string.Empty,
                Price = classRoom.Course.Price,
            });

            await _redisDb.StringSetAsync(
                new RedisKey(key),
                new RedisValue(JsonSerializer.Serialize(cartItems)),
                TimeSpan.FromMinutes(30));
        }

        /// <summary>
        /// Delete the CartDatabase 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task DeleteCartAsync(int userId)
        {
            var key = string.Format(AppCts.RedisDatabase.Cart.KeyTemplate, userId);
            await _redisDb.KeyDeleteAsync(key);
        }

        /// <summary>
        /// Get the CartDatabase Items
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<CartItem>> GetCartItemsAsync(int userId)
        {
            var key = string.Format(AppCts.RedisDatabase.Cart.KeyTemplate, userId);
            var cartList = await _redisDb.StringGetAsync(key);
            return string.IsNullOrEmpty(cartList)
                ? []
                : JsonSerializer.Deserialize<List<CartItem>>(cartList) ?? [];
        }
    }
}
