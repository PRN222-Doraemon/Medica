using Core.Entities;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.Checkout;

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
                        await HandleCreatePendingOrder(stripeEvent);
                        break;

                    //// Creating Order with Status = Paid
                    case EventTypes.PaymentIntentSucceeded:
                        await HandleUpdateOrderPaid(stripeEvent);
                        break;

                    // Update Order with Status = Cancelled if any frauds occurs
                    case EventTypes.PaymentIntentCanceled:
                    case EventTypes.CheckoutSessionExpired:
                    case EventTypes.PaymentIntentPaymentFailed:
                        await HandleUpdateOrderCancelled(stripeEvent);
                        break;
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }

        // ==============================
        // === Webhook Event from Stripe 
        // ==============================

        private async Task HandleCreatePendingOrder(Event stripeEvent)
        {
            var checkoutSession = stripeEvent.Data.Object as Stripe.Checkout.Session;
            if (checkoutSession == null) return;

            var paymentIntentId = checkoutSession.PaymentIntentId;
            var order = await _orderService.CreateOrderFromCartAsync(paymentIntentId, int.Parse(checkoutSession.Metadata["UserId"]));

            switch (checkoutSession.PaymentStatus)
            {
                case "paid":
                    {
                        // If the event paid fires faster then the completion of session checkout
                        await _orderService.UpdateOrderStatusAsync(order, OrderStatus.Paid);
                        break;
                    }
                default:
                    {
                        await _orderService.UpdateOrderStatusAsync(order, OrderStatus.Pending);
                        break;
                    }
            }
        }

        private async Task HandleUpdateOrderPaid(Event stripeEvent)
        {
            var checkoutSession = stripeEvent.Data.Object as Stripe.Checkout.Session;
            if (checkoutSession == null) return;

            var paymentIntentId = checkoutSession.PaymentIntentId;
            var order = await _orderService.GetOrderByPaymentIntentIdAsync(paymentIntentId);
            if (order == null) return;

            if (order.Status != OrderStatus.Paid)
            {
                await _orderService.UpdateOrderStatusAsync(order, OrderStatus.Paid);
            }
        }

        private async Task HandleUpdateOrderCancelled(Event stripeEvent)
        {
            var checkoutSession = stripeEvent.Data.Object as Stripe.Checkout.Session;
            if (checkoutSession == null) return;

            var paymentIntentId = checkoutSession.PaymentIntentId;
            var order = await _orderService.GetOrderByPaymentIntentIdAsync(paymentIntentId);
            if (order == null) return;

            if (order.Status != OrderStatus.Cancelled)
            {
                await _orderService.UpdateOrderStatusAsync(order, OrderStatus.Cancelled);
            }
        }
    }
}
