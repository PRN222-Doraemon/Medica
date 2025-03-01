using Core.Entities;
using Core.Interfaces.Repos;
using Core.Interfaces.Services;
using Core.Specifications.Courses;
using Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class StripePaymentService : IPaymentService
    {
        // ==============================
        // === Fields & Props
        // ==============================

        private readonly IUnitOfWork _unitOfWork;

        // ==============================
        // === Constructors
        // ==============================

        public StripePaymentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ==============================
        // === Methods
        // ==============================

        public async Task<string> CreateCheckoutSessionAsync(List<CartItem> cartItems, int userId, string successfulUrl, string cancelUrl)
        {
            // If cart items is empty
            if (!cartItems.Any()) throw new ArgumentException("Invalid Cart for checkout");

            // Stripe Options 
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = successfulUrl,
                CancelUrl = cancelUrl,
                ExpiresAt = DateTime.UtcNow.AddMinutes(30),
                Metadata = new Dictionary<string, string> { { "UserId", userId.ToString() } }
            };

            // Config the item listing on course session
            foreach (var item in cartItems)
            {
                var course = await _unitOfWork.Repository<Course>()
                    .GetEntityWithSpec(new CourseSpecification(item.CourseId));

                if (course == null) continue;

                options.LineItems.Add(new SessionLineItemOptions
                {
                    Quantity = 1,
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "vnd",
                        UnitAmount = (long)item.Price,

                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = course.Name,
                            Description = course.Description,
                            Images = new List<string> { course.ImgUrl },
                        }
                    }
                });
            }

            var sessionService = new SessionService();
            var session = await sessionService.CreateAsync(options);
            return session.Id;
        }
    }
}
