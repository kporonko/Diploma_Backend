using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Application.Repositories.Payment.Proxies;
using Diploma.Backend.Infrastructure.PayPal.Facades;
using Diploma.Backend.Infrastructure.PayPal.Helpers;
using Diploma.Backend.Infrastructure.PayPal.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Infrastructure.PayPal.Proxies.impl
{
    public class PayPalProxy : IPaymentProxy
    {
        private readonly IPayPalFacade _payPalFacade;
        private readonly PayPalConfig _configuration;
        private readonly IConfiguration _configurationExtension;

        public PayPalProxy(IConfiguration configuration, IPayPalFacade payPalFacade)
        {
            _configuration = new PayPalConfig(configuration);
            _payPalFacade = payPalFacade;
            _configurationExtension = configuration;
        }

        public async Task<PayPalAccessTokenResponse> GetTokenAsync()
        {
            var tokenAuthBasic = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_configuration.ClientId}:{_configuration.ClientSecret}"));
            var tokenResponse = _payPalFacade.Post<PayPalAccessTokenResponse>(
                _configuration.TokenUrl,
                new Dictionary<string, string> { { "grant_type", "client_credentials" } },
                null,
                new Dictionary<string, string> { { "Authorization", $"Basic {tokenAuthBasic}" } },
                "application/x-www-form-urlencoded"
            );

            return await Task.FromResult(tokenResponse);
        }

        public async Task<PayPalSubscriptionResponse> CreateSubscriptionAsync(PayPalSubscriptionRequestShort request, string token, string planId)
        {
            var requestPayPal = new PayPalRequestGenerator(_configurationExtension).GenerateSubscriptionRequest();
            requestPayPal.plan_id = planId;
            requestPayPal.start_time = DateTime.UtcNow.AddMinutes(1);
            requestPayPal.application_context.return_url = request.ReturnUrl;
            requestPayPal.application_context.cancel_url = request.CancelUrl;
            return await Task.FromResult(_payPalFacade.Post<PayPalSubscriptionResponse>(
                _configuration.CreateSubscriptionUrl,
                requestPayPal,
                null,
                new Dictionary<string, string> { { "Authorization", $"Bearer {token}" } }
            ));
        }

        public async Task<PayPalSubscriptionResponse> GetSubscriptionAsync(string id, string token)
        {
            var url = _configuration.GetSubscriptionUrl.Replace("{id}", id);
            return await Task.FromResult(_payPalFacade.Get<PayPalSubscriptionResponse>(
                url,
                null,
                null,
                new Dictionary<string, string> { { "Authorization", $"Bearer {token}" } }
            ));
        }

        public async Task<PayPalCancelSubscriptionResponse> CancelSubscriptionAsync(string id, PayPalCancelSubscriptionRequest request, string token)
        {
            var cancelSubscriptionUrl = _configuration.CancelSubscriptionUrl.Replace("{id}", id);
            return await Task.FromResult(_payPalFacade.Post<PayPalCancelSubscriptionResponse>(
                cancelSubscriptionUrl,
                request,
                null,
                new Dictionary<string, string> { { "Authorization", $"Bearer {token}" } }
            ));
        }

        public async Task<PayPalSubscriptionResponse> ActivateSubscriptionAsync(string id, ActivateSubscriptionRequest request, string token)
        {
            var url = _configuration.ActivateSubscriptionUrl.Replace("{id}", id);
            return await Task.FromResult(_payPalFacade.Post<PayPalSubscriptionResponse>(
                url,
                request,
                null,
                new Dictionary<string, string> { { "Authorization", $"Bearer {token}" } }
            ));
        }

        public async Task<PayPalPlanResponse> CreatePlanAsync(string token, string productId)
        {
            var request = new PayPalRequestGenerator(_configurationExtension).GeneratePlanRequest();
            request.ProductId = productId;
            return await Task.FromResult(_payPalFacade.Post<PayPalPlanResponse>(
                _configuration.CreatePlanUrl,
                request,
                null,
                new Dictionary<string, string> { { "Authorization", $"Bearer {token}" } }
            ));
        }

        public async Task<PayPalProductResponse> CreateProductAsync(string token)
        {
            var requestPayPal = new PayPalRequestGenerator(_configurationExtension).GenerateProductRequest();
            return await Task.FromResult(_payPalFacade.Post<PayPalProductResponse>(
                _configuration.CreateProductUrl,
                requestPayPal,
                null,
                new Dictionary<string, string> { { "Authorization", $"Bearer {token}" } }
            ));
        }
    }
}
