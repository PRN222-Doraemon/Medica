﻿using Core.Entities;
using Core.Interfaces.Repos;
using Core.Interfaces.Services;
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

        private readonly IConfiguration _config;
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

            // Retrieve all courses based on courseId

            // Stripe Options 
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = cartItems.Select(item => new SessionLineItemOptions
                {
                    Quantity = 1,
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "vnd",
                        UnitAmount = (long)item.Price,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.CourseName,
                            Description = "Course description"
                        }
                    }
                }).ToList(),
                Mode = "payment",
                SuccessUrl = successfulUrl,
                CancelUrl = cancelUrl,
                Metadata = new Dictionary<string, string> { { "UserId", userId.ToString() } }
            };

            var sessionService = new SessionService();
            var session = await sessionService.CreateAsync(options);
            return session.Id;
        }

        public async Task<string> VerifyPaymentAsync(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                throw new ArgumentException("Session Id is null");
            }

            var sessionService = new SessionService();
            var session = await sessionService.GetAsync(sessionId);
            return null;
        }
    }
}
