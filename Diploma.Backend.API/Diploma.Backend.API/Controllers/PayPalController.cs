using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Application.Helpers;
using Diploma.Backend.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Security.Claims;

namespace Diploma.Backend.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PayPalController : ControllerBase
    {
        private readonly IPayPalService _payPalService;

        public PayPalController(IPayPalService payPalService)
        {
            _payPalService = payPalService;
        }

        [HttpPost]
        [Route("create-plan")]
        public async Task<IActionResult> CreatePlan([FromBody] PayPalPlanRequest request)
        {
            var response = await _payPalService.CreatePlan(request);
            return Ok(response);
        }

        [HttpPost]
        [Route("create-product")]
        public async Task<IActionResult> CreateProduct([FromBody] PayPalProductRequest request)
        {
            var response = await _payPalService.CreateProduct(request);
            return Ok(response);
        }

        
        [HttpPost]
        [Route("create-subscription")]
        public async Task<IActionResult> CreateSubscription([FromBody] PayPalSubscriptionRequest request)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var user = CurrentUserRetriever.GetCurrentUser(identity);
            if (user.Error != null)
            {
                return Unauthorized(user);
            }
            var response = await _payPalService.CreateSubscription(request, user.Data);
            return Ok(response);
        }

        [HttpPost]
        [Route("cancel/{id}")]
        public async Task<IActionResult> CancelSubscription([FromBody] PayPalCancelSubscriptionRequest request, [FromRoute] string id)
        {
            var response = await _payPalService.CancelSubscription(id, request);
            return Ok(response);
        }

        [HttpGet]
        [Route("subscription/{id}")]
        public async Task<IActionResult> GetSubscription([FromRoute] string id)
        {
            var response = await _payPalService.GetSubscription(id);
            return Ok(response);
        }

        [HttpPost]
        [Route("activate/{id}")]
        public async Task<IActionResult> ActivateSubscription([FromRoute] string id, [FromBody] ActivateSubscriptionRequest request)
        {
            var response = await _payPalService.ActivateSubscription(id, request);
            return Ok(response);
        }
        
        [HttpPost]
        [Route("capture-payment/{subscrId}")]
        public async Task<IActionResult> CapturePayment([FromRoute] string subscrId, [FromBody] PayPalPaymentRequest request)
        {
            var response = await _payPalService.CapturePayment(subscrId, request);
            return Ok(response);
        }

        [HttpPost]
        [Route("paypal-webhook")]
        public async Task<IActionResult> ReceiveWebhook()
        {
            try
            {
                string requestBody;
                using (var reader = new StreamReader(Request.Body))
                {
                    requestBody = await reader.ReadToEndAsync();
                }

                var request = JObject.Parse(requestBody);
                var eventType = request.Value<string>("event_type");
                var subscriptionId = request.SelectToken("resource.id")?.Value<string>();

                if (eventType == "BILLING.SUBSCRIPTION.ACTIVATED")
                {
                    HandleSubscriptionActivation(subscriptionId);
                }
                else if (eventType == "BILLING.SUBSCRIPTION.EXPIRED")
                {
                    HandleSubscriptionExpiration(subscriptionId);
                }
                return Ok("Webhook received and processed");
            }
            catch (Exception)
            {
                return Ok("Webhook received and processed");
            }
        }

        private void HandleSubscriptionExpiration(string? subscriptionId)
        {
            _payPalService.HandleExpiration(subscriptionId);
        }

        private void HandleSubscriptionActivation(string? subscriptionId)
        {
            _payPalService.HandleActivation(subscriptionId);
        }
    }

    
}
