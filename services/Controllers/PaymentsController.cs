using System.IO;
using System.Threading.Tasks;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using services.Errors;
using Stripe;

namespace services.Controllers
{
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;

        private const string WhSecret = "whsec_Uqumw6weAgn6ODcLfUpRS4mUdJYWjdfH";
        private readonly ILogger<IPaymentService> _logger;

        public PaymentsController(IPaymentService paymentService, ILogger<IPaymentService> logger)
        {
            _logger = logger;
            _paymentService = paymentService;

        }

        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<Core.Entities.CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (basket == null)
            {
                return BadRequest(new ServiceResponse(400, "Problem with your basket"));
            }

            return basket;
        }

        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhook() {

            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], WhSecret);

            PaymentIntent paymentIntent;
            Core.Entities.OrderAggregate.Order order;
            switch(stripeEvent.Type) {
                case "payment_intent.succeeded":
                    paymentIntent = (PaymentIntent) stripeEvent.Data.Object;
                    _logger.LogInformation("Payment succeded: ", paymentIntent.Id);
                    order = await _paymentService.UpdateOrderStatus(paymentIntent.Id, OrderStatus.PaymentReceived);
                    _logger.LogInformation("Order updated to payment received: ", order.Id);
                    break;
                case "payment_intent.payment_failed":
                    paymentIntent = (PaymentIntent) stripeEvent.Data.Object;
                    _logger.LogInformation("Payment failed: ", paymentIntent.Id);
                    order = await _paymentService.UpdateOrderStatus(paymentIntent.Id, OrderStatus.PaymentFailed);
                    _logger.LogInformation("Order updated to payment failed: ", order.Id);
                    break;
            }            
            
            return new EmptyResult();
        }

    }
}