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
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _payPalService;
        public PaymentController(IPaymentService payPalService)
        {
            _payPalService = payPalService;
        }

        [HttpPost]
        [Route("subscription")]
        public async Task<IActionResult> CreateSubscription([FromBody] PayPalSubscriptionRequestShort request)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var user = CurrentUserRetriever.GetCurrentUser(identity);
            if (user.Error != null)
            {
                return Unauthorized(user);
            }

            var response = await _payPalService.CreateSubscription(request, user.Data);
            if (response.Error != null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("cancel/{id}")]
        public async Task<IActionResult> CancelSubscription([FromBody] PayPalCancelSubscriptionRequest request, [FromRoute] string id)
        {
            var response = await _payPalService.CancelSubscription(id, request);
            if (response.Error != null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("subscription/{id}")]
        public async Task<IActionResult> GetSubscription([FromRoute] string id)
        {
            var response = await _payPalService.GetSubscription(id);
            if (response.Error != null)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("activate/{id}")]
        public async Task<IActionResult> ActivateSubscription([FromRoute] string id, [FromBody] ActivateSubscriptionRequest request)
        {
            var response = await _payPalService.ActivateSubscription(id, request);
            if (response.Error != null)
            {
                return BadRequest(response);
            }
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

                if (eventType == "BILLING.SUBSCRIPTION.EXPIRED" || eventType == "BILLING.SUBSCRIPTION.CANCELLED")
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
    }
}
