using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Application.Repositories.Payment.Proxies;
using Diploma.Backend.Infrastructure.PayPal.Facades;
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

        public PayPalProxy(IConfiguration configuration, IPayPalFacade payPalFacade)
        {
            _configuration = new PayPalConfig(configuration);
            _payPalFacade = payPalFacade;
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

        public async Task<PayPalSubscriptionResponse> CreateSubscriptionAsync(PayPalSubscriptionRequest request, string token)
        {
            return await Task.FromResult(_payPalFacade.Post<PayPalSubscriptionResponse>(
                _configuration.CreateSubscriptionUrl,
                request,
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

        public async Task<PayPalPlanResponse> CreatePlanAsync(PayPalPlanRequest request, string token)
        {
            return await Task.FromResult(_payPalFacade.Post<PayPalPlanResponse>(
                _configuration.CreatePlanUrl,
                request,
                null,
                new Dictionary<string, string> { { "Authorization", $"Bearer {token}" } }
            ));
        }

        public async Task<PayPalPaymentResponse> CapturePaymentAsync(string subscrId, PayPalPaymentRequest request, string token)
        {
            var url = _configuration.CapturePaymentUrl.Replace("{id}", subscrId);
            return await Task.FromResult(_payPalFacade.Post<PayPalPaymentResponse>(
                url,
                request,
                null,
                new Dictionary<string, string> { { "Authorization", $"Bearer {token}" } }
            ));
        }

        public async Task<PayPalProductResponse> CreateProductAsync(PayPalProductRequest request, string token)
        {
            return await Task.FromResult(_payPalFacade.Post<PayPalProductResponse>(
                _configuration.CreateProductUrl,
                request,
                null,
                new Dictionary<string, string> { { "Authorization", $"Bearer {token}" } }
            ));
        }
    }
}
