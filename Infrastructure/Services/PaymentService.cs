using Core.Entities;
using Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class StripePaymentService : IPaymentService
    {
        public Task<string> CreateCheckoutSessionAsync(List<CartItem> cartItems, int userId, string successfulUrl, string cancelUrl)
        {
            throw new NotImplementedException();
        }

        public Task<string> VerifyPaymentAsync(string sessionId)
        {
            throw new NotImplementedException();
        }
    }
}
