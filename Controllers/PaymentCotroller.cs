using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Service.Services.CustomerBasketService.Dtos;
using Store.Service.Services.OrderService.DTOS;
using Store.Service.Services.PaymentService;
using Stripe;

namespace STORE.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentCotroller : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentCotroller> _logger;
        private string endpointSecret = "whsec_2a83b00c6518cef2d7ca20f70e50509deca213d85bd140f318a6c97f527e337a";
        public PaymentCotroller(IPaymentService paymentService,ILogger<PaymentCotroller> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }


        [HttpPost("ExistingOrder/{basketId}")]

        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntentForExistingOrder(CustomerBasketDto input)
              =>   Ok(await _paymentService.CreateOrUpdatePaymentIntentForExistingOrder(input));

        [HttpPost("NewOrder/{basketId}")]

        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntentForNewOrder(string basketId)
             => Ok(await _paymentService.CreateOrUpdatePaymentIntentForNewOrder(basketId));





        /*the url https://localhost:7235/api/PaymentCotroller/webhook*/
        //signing secret is whsec_2a83b00c6518cef2d7ca20f70e50509deca213d85bd140f318a6c97f527e337a (^C to quit)
        [HttpPost("webhook")]

        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);
                PaymentIntent paymentIntent;
                OrderResultDto order;
                // Handle the event

                if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                {

                    paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment Failed : ",paymentIntent.Id);
                    order = await _paymentService.UpdateOrderPaymentFailed(paymentIntent.Id);
                    _logger.LogInformation("Order Updated To Payment Failed",order.Id);

                }
                else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment Succeed : ", paymentIntent.Id);
                    order = await _paymentService.UpdateOrderPaymentSucceeded(paymentIntent.Id);
                    _logger.LogInformation("Order Updated To Payment Succeeded", order.Id);
                }
                // ... handle other event types
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }
            }
            catch (StripeException e)
            {
                // ...
            }

            return Ok();
        }

    }
}
