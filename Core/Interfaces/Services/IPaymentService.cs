using Core.Entities;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<string> CreateCheckoutSessionAsync(List<CartItem> cartItems, int userId, string successfulUrl, string cancelUrl);
    }
}
