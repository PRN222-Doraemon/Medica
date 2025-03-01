using Core.Entities;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.Checkout;
using Stripe.V2;
using Stripe.V2.Core;

namespace MedicaWeb_MVC.Controllers
{
    [ApiController]
    [Route("api/webhook")]
    public class WebhookController : Controller
    {
        // ==============================
        // === Fields & Props
        // ==============================

        private readonly IConfiguration _configuration;
        private readonly IOrderService _orderService;

        // ==============================
        // === Constructors
        // ==============================

        public WebhookController(IConfiguration configuration, IOrderService orderService)
        {
            _configuration = configuration;
            _orderService = orderService;

        }

        // ==============================
        // === Methods
        // ==============================

        /// <summary>
        /// Automaticcally receive the webhook event sent from the Stripe
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var endpointSecret = _configuration["Stripe:WebhookSecret"];
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], endpointSecret);

                switch (stripeEvent.Type)
                {
                    // Creating Order with Status = Pending
                    case EventTypes.CheckoutSessionCompleted:
                        var checkoutSession = stripeEvent.Data.Object as Stripe.Checkout.Session;
                        await _orderService.CreateOrderFromCartAsync(int.Parse(checkoutSession.Metadata["UserId"]), checkoutSession.PaymentIntentId);
                        break;

                    //// Update Order with Status = Paid
                    //case EventTypes.PaymentIntentSucceeded:
                    //    await _orderService.UpdateOrderStatusAsync(int.Parse(session.Metadata["UserId"]), OrderStatus.Paid);
                    //    break;

                    //// Update Order with Status = Cancel
                    //case Events.PaymentIntentCanceled:
                    //case Events.CheckoutSessionExpired:
                    //case Events.PaymentIntentPaymentFailed:
                    //    await Handle
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }
    }
}
