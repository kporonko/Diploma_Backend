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
    /// <summary>
    /// Service for interacting with PayPal API.
    /// </summary>
    public class PayPalService : IPayPalService
    {
        private readonly IPayPalFacade _payPalFacade;
        private readonly PayPalConfig _configuration;
        private readonly ApplicationContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="PayPalService"/> class.
        /// </summary>
        /// <param name="configuration">The configuration settings.</param>
        /// <param name="payPalFacade">The PayPal facade.</param>
        /// <param name="applicationContext">The application context.</param>
        public PayPalService(IConfiguration configuration, IPayPalFacade payPalFacade, ApplicationContext applicationContext)
        {
            _db = applicationContext;
            _configuration = new PayPalConfig(configuration);
            _payPalFacade = payPalFacade;
        }

        /// <summary>
        /// Retrieves an access token from PayPal.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the access token response.</returns>
        private async Task<PayPalAccessTokenResponse> GetTokenAsync()
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

        /// <summary>
        /// Cancels a subscription.
        /// </summary>
        /// <param name="id">The subscription ID.</param>
        /// <param name="request">The cancellation request.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the base response with cancellation details.</returns>
        public async Task<BaseResponse<PayPalCancelSubscriptionResponse>> CancelSubscription(string id, PayPalCancelSubscriptionRequest request)
        {
            var token = await GetTokenAsync();
            var cancelSubscriptionUrl = _configuration.CancelSubscriptionUrl.Replace("{id}", id);

            var cancelResponse = _payPalFacade.Post<PayPalCancelSubscriptionResponse>(
                cancelSubscriptionUrl,
                request,
                null,
                GetAuthHeader(token.access_token)
            );

            return BaseResponseGenerator.GenerateValidBaseResponse(cancelResponse);
        }

        /// <summary>
        /// Generates the authorization header with the given token.
        /// </summary>
        /// <param name="token">The access token.</param>
        /// <returns>The authorization header.</returns>
        private Dictionary<string, string> GetAuthHeader(string token)
        {
            return new Dictionary<string, string> { { "Authorization", $"Bearer {token}" } };
        }

        /// <summary>
        /// Creates a new subscription.
        /// </summary>
        /// <param name="request">The subscription request.</param>
        /// <param name="jwtUser">The user initiating the request.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the base response with subscription details.</returns>
        public async Task<BaseResponse<PayPalSubscriptionResponse>> CreateSubscription(PayPalSubscriptionRequest request, User jwtUser)
        {
            var token = await GetTokenAsync();

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
                DateCreate = subscrResponse.create_time,
                DateChangeStatus = subscrResponse.status_update_time >= subscrResponse.create_time ? subscrResponse.status_update_time : subscrResponse.create_time,
                SubscriptionId = subscrResponse.id,
            };

            _db.Subscriptions.Add(subscription);
            await _db.SaveChangesAsync();

            return BaseResponseGenerator.GenerateValidBaseResponse(subscrResponse);
        }

        /// <summary>
        /// Retrieves subscription details.
        /// </summary>
        /// <param name="id">The subscription ID.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the base response with subscription details.</returns>
        public async Task<BaseResponse<PayPalSubscriptionResponse>> GetSubscription(string id)
        {
            var token = await GetTokenAsync();
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

        /// <summary>
        /// Activates a subscription.
        /// </summary>
        /// <param name="id">The subscription ID.</param>
        /// <param name="request">The activation request.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the base response with subscription details.</returns>
        public async Task<BaseResponse<PayPalSubscriptionResponse>> ActivateSubscription(string id, ActivateSubscriptionRequest request)
        {
            var token = await GetTokenAsync();
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

        /// <summary>
        /// Creates a new PayPal plan.
        /// </summary>
        /// <param name="request">The plan creation request.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the base response with plan details.</returns>
        public async Task<BaseResponse<PayPalPlanResponse>> CreatePlan(PayPalPlanRequest request)
        {
            var token = await GetTokenAsync();
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

        /// <summary>
        /// Captures a payment for a subscription.
        /// </summary>
        /// <param name="subscrId">The subscription ID.</param>
        /// <param name="request">The payment request.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the base response with payment details.</returns>
        public async Task<BaseResponse<PayPalPaymentResponse>> CapturePayment(string subscrId, PayPalPaymentRequest request)
        {
            var token = await GetTokenAsync();
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

        /// <summary>
        /// Creates a new PayPal product.
        /// </summary>
        /// <param name="request">The product creation request.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the base response with product details.</returns>
        public async Task<BaseResponse<PayPalProductResponse>> CreateProduct(PayPalProductRequest request)
        {
            var token = await GetTokenAsync();
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

        /// <summary>
        /// Handles the activation of a subscription.
        /// </summary>
        /// <param name="id">The subscription ID.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task HandleActivation(string id)
        {
            var subscription = _db.Subscriptions.FirstOrDefault(s => s.SubscriptionId == id);
            subscription.IsActive = true;
            subscription.DateChangeStatus = DateTime.Now;
            _db.Subscriptions.Update(subscription);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Handles the expiration of a subscription.
        /// </summary>
        /// <param name="id">The subscription ID.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
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
