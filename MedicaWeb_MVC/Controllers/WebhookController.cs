using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        // ==============================
        // === Constructors
        // ==============================

        public WebhookController(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        // ==============================
        // === Methods
        // ==============================


        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var endpointSecret = _configuration["Stripe:WebhookSecret"];
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], endpointSecret);
                var checkoutSession = stripeEvent.Data.Object as Stripe.Checkout.Session;

                switch (stripeEvent.Type)
                {
                    // Handle if the 
                    case EventTypes.CheckoutSessionCompleted:
                        await HandleCheckoutSessionCompleted(checkoutSession);
                        break;

                    // Handle if the checkout expires
                    case EventTypes.CheckoutSessionExpired:
                        await HandleCheckoutSessionExpired(checkoutSession);
                        break;
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }

        private async Task HandleCheckoutSessionExpired(Session? checkoutSession)
        {

        }

        private async Task HandleCheckoutSessionCompleted(Session? checkoutSession)
        {

        }
    }
}
