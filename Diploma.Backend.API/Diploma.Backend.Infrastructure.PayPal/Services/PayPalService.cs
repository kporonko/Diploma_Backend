using Azure;
using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Application.Helpers;
using Diploma.Backend.Application.Mappers;
using Diploma.Backend.Application.Services;
using Diploma.Backend.Domain.Common;
using Diploma.Backend.Domain.Models;
using Diploma.Backend.Infrastructure.Data;
using Diploma.Backend.Infrastructure.PayPal.Facades;
using Diploma.Backend.Infrastructure.PayPal.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Infrastructure.PayPal.Services
{
    public class PayPalService : IPayPalService
    {
        private readonly IPayPalFacade _payPalFacade;
        private readonly PayPalConfig _configuration;
        private readonly ApplicationContext _db;

        public PayPalService(IConfiguration configuration, IPayPalFacade payPalFacade, ApplicationContext applicationContext)
        {
            _db = applicationContext;
            _configuration = new PayPalConfig(configuration);
            _payPalFacade = payPalFacade;
        }
        public Task<PayPalAccessTokenResponse> GetToken()
        {
            PayPalAccessTokenResponse tokenResponse = _payPalFacade.Post<PayPalAccessTokenResponse>(
                _configuration.TokenUrl,
                new Dictionary<string, string>
                {
                    { "grant_type", "client_credentials" }
                },
                null,
                new Dictionary<string, string>
                {
                    { "Authorization", $"Basic {Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_configuration.ClientId}:{_configuration.ClientSecret}"))}" }
                },
                "application/x-www-form-urlencoded"
                );

            return Task.FromResult(tokenResponse);
        }
        
        public async Task<BaseResponse<PayPalCancelSubscriptionResponse>> CancelSubscription(string id, PayPalCancelSubscriptionRequest request)
        {
            var token = await GetToken();

            string cancelSubscriptionUrl = _configuration.CancelSubscriptionUrl.Replace("{id}", id);

            PayPalCancelSubscriptionResponse cancelResponse = _payPalFacade.Post<PayPalCancelSubscriptionResponse>(
                cancelSubscriptionUrl,
                request,
                null,
                new Dictionary<string, string>
                {
                    { "Authorization", $"Bearer {token.access_token}" }
                });

            return BaseResponseGenerator.GenerateValidBaseResponse(cancelResponse);
        }

        public async Task<BaseResponse<PayPalSubscriptionResponse>> CreateSubscription(PayPalSubscriptionRequest request, User jwtUser)
        {
            var token = await GetToken();

            PayPalSubscriptionResponse subscrResponse = _payPalFacade.Post<PayPalSubscriptionResponse>(
                _configuration.CreateSubscriptionUrl,
                request,
                null,
                new Dictionary<string, string>
                {
                    { "Authorization", $"Bearer {token.access_token}" }
                });

            var user = _db.Users.FirstOrDefault(u => u.Id == jwtUser.Id);

            var subscription = new Subscription
            {
                User = user,
                UserId = user.Id,
                IsActive = false,
                Price = Convert.ToInt64(subscrResponse.shipping_amount.value),
                CurrencyCode = subscrResponse.shipping_amount.currency_code,
                DateCreate = subscrResponse.create_time,
                DateChangeStatus = subscrResponse.status_update_time,
                SubscriptionId = subscrResponse.id,
            };

            _db.Subscriptions.Add(subscription);
            await _db.SaveChangesAsync();

            return BaseResponseGenerator.GenerateValidBaseResponse(subscrResponse);
        }

        public async Task<BaseResponse<PayPalSubscriptionResponse>> GetSubscription(string id)
        {
            var token = await GetToken();
            var url = _configuration.GetSubscriptionUrl.Replace("{id}", id);
            PayPalSubscriptionResponse subscrResponse = _payPalFacade.Get<PayPalSubscriptionResponse>(
                url,
                null,
                null,
                new Dictionary<string, string>
                {
                    { "Authorization", $"Bearer {token.access_token}" }
                });

            return BaseResponseGenerator.GenerateValidBaseResponse(subscrResponse);
        }

        public async Task<BaseResponse<PayPalSubscriptionResponse>> ActivateSubscription(string id, ActivateSubscriptionRequest request)
        {
            var token = await GetToken();
            var url = _configuration.ActivateSubscriptionUrl.Replace("{id}", id);
            PayPalSubscriptionResponse subscrResponse = _payPalFacade.Post<PayPalSubscriptionResponse>(
                url,
                request,
                null,
                new Dictionary<string, string>
                {
                    { "Authorization", $"Bearer {token.access_token}" }
                });

            return BaseResponseGenerator.GenerateValidBaseResponse(subscrResponse);
        }

        public async Task<BaseResponse<PayPalPlanResponse>> CreatePlan(PayPalPlanRequest request)
        {
            var token = await GetToken();
            PayPalPlanResponse subscrResponse = _payPalFacade.Post<PayPalPlanResponse>(
                $"{_configuration.CreatePlanUrl}",
                request,
                null,
                new Dictionary<string, string>
                {
                    { "Authorization", $"Bearer {token.access_token}" }
                });

            return BaseResponseGenerator.GenerateValidBaseResponse(subscrResponse);
        }

        public async Task<BaseResponse<PayPalPaymentResponse>> CapturePayment(string subscrId, PayPalPaymentRequest request)
        {
            var token = await GetToken();
            var url = _configuration.CapturePaymentUrl.Replace("{id}", subscrId);
            PayPalPaymentResponse subscrResponse = _payPalFacade.Post<PayPalPaymentResponse>(
                url,
                request,
                null,
                new Dictionary<string, string>
                {
                    { "Authorization", $"Bearer {token.access_token}" }
                });

            return BaseResponseGenerator.GenerateValidBaseResponse(subscrResponse);
        }

        public async Task<BaseResponse<PayPalProductResponse>> CreateProduct(PayPalProductRequest request)
        {
            var token = await GetToken();
            PayPalProductResponse subscrResponse = _payPalFacade.Post<PayPalProductResponse>(
                _configuration.CreateProductUrl,
                request,
                null,
                new Dictionary<string, string>
                {
                    { "Authorization", $"Bearer {token.access_token}" }
                });

            return BaseResponseGenerator.GenerateValidBaseResponse(subscrResponse);
        }

        public async Task HandleActivation(string id)
        {
            var subscription = _db.Subscriptions.FirstOrDefault(s => s.SubscriptionId == id);
            subscription.IsActive = true;
            subscription.DateChangeStatus = DateTime.Now;
            _db.Subscriptions.Update(subscription);
            await _db.SaveChangesAsync();
        }

        public async Task HandleExpiration(string id)
        {
            var subscription = _db.Subscriptions.FirstOrDefault(s => s.SubscriptionId == id);
            subscription.IsActive = false;
            subscription.DateChangeStatus = DateTime.Now;
            _db.Subscriptions.Update(subscription);
            await _db.SaveChangesAsync();
        }
    }
}
