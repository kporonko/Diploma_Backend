using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Application.Helpers;
using Diploma.Backend.Application.Mappers;
using Diploma.Backend.Application.Repositories.Payment;
using Diploma.Backend.Application.Repositories.Payment.Proxies;
using Diploma.Backend.Application.Services;
using Diploma.Backend.Domain.Common;
using Diploma.Backend.Domain.Enums;
using Diploma.Backend.Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Services.Payment.impl
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentProxy _payPalProxy;
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentProxy paymentProxy, IPaymentRepository paymentRepository)
        {
            _payPalProxy = paymentProxy;
            _paymentRepository = paymentRepository;
        }

        public async Task<BaseResponse<PayPalCancelSubscriptionResponse>> CancelSubscription(string id, PayPalCancelSubscriptionRequest request)
        {
            var token = await _payPalProxy.GetTokenAsync();
            var cancelResponse = await _payPalProxy.CancelSubscriptionAsync(id, request, token.access_token);

            return BaseResponseGenerator.GenerateValidBaseResponse(cancelResponse);
        }

        public async Task<BaseResponse<PayPalSubscriptionResponse>> CreateSubscription(PayPalSubscriptionRequest request, User jwtUser)
        {
            var existingSubscription = _paymentRepository.GetSubscriptionByUserId(jwtUser.Id);
            if (existingSubscription != null && existingSubscription.IsActive)
            {
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<PayPalSubscriptionResponse>(ErrorCodes.SubscriptionAlreadyExists.ToString());
            }
            var token = await _payPalProxy.GetTokenAsync();
            var subscrResponse = await _payPalProxy.CreateSubscriptionAsync(request, token.access_token);

            var user = _paymentRepository.GetUserById(jwtUser.Id);

            var subscription = new Subscription
            {
                User = user,
                UserId = user.Id,
                IsActive = false,
                DateCreate = subscrResponse.create_time,
                DateChangeStatus = subscrResponse.status_update_time >= subscrResponse.create_time ? subscrResponse.status_update_time : subscrResponse.create_time,
                SubscriptionId = subscrResponse.id,
            };

            await _paymentRepository.AddSubscriptionAsync(subscription);

            return BaseResponseGenerator.GenerateValidBaseResponse(subscrResponse);
        }

        public async Task<BaseResponse<PayPalSubscriptionResponse>> GetSubscription(string id)
        {
            var token = await _payPalProxy.GetTokenAsync();
            var subscrResponse = await _payPalProxy.GetSubscriptionAsync(id, token.access_token);

            return BaseResponseGenerator.GenerateValidBaseResponse(subscrResponse);
        }

        public async Task<BaseResponse<PayPalSubscriptionResponse>> ActivateSubscription(string id, ActivateSubscriptionRequest request)
        {
            var token = await _payPalProxy.GetTokenAsync();
            var subscrResponse = await _payPalProxy.ActivateSubscriptionAsync(id, request, token.access_token);

            var subscription = _paymentRepository.GetSubscriptionById(id);
            subscription.IsActive = true;
            subscription.DateChangeStatus = subscrResponse.status_update_time;

            _paymentRepository.UpdateSubscription(subscription);

            return BaseResponseGenerator.GenerateValidBaseResponse(subscrResponse);
        }

        public async Task<BaseResponse<PayPalPlanResponse>> CreatePlan(PayPalPlanRequest request)
        {
            var token = await _payPalProxy.GetTokenAsync();
            var planResponse = await _payPalProxy.CreatePlanAsync(request, token.access_token);

            return BaseResponseGenerator.GenerateValidBaseResponse(planResponse);
        }

        public async Task<BaseResponse<PayPalPaymentResponse>> CapturePayment(string subscrId, PayPalPaymentRequest request)
        {
            var token = await _payPalProxy.GetTokenAsync();
            var paymentResponse = await _payPalProxy.CapturePaymentAsync(subscrId, request, token.access_token);

            var subscription = _paymentRepository.GetSubscriptionById(subscrId);
            subscription.IsActive = true;
            subscription.DateChangeStatus = DateTime.UtcNow;

            _paymentRepository.UpdateSubscription(subscription);

            return BaseResponseGenerator.GenerateValidBaseResponse(paymentResponse);
        }

        public async Task<BaseResponse<PayPalProductResponse>> CreateProduct(PayPalProductRequest request)
        {
            var token = await _payPalProxy.GetTokenAsync();
            var productResponse = await _payPalProxy.CreateProductAsync(request, token.access_token);

            return BaseResponseGenerator.GenerateValidBaseResponse(productResponse);
        }

        public async Task HandleActivation(string id)
        {
            var subscription = _paymentRepository.GetSubscriptionById(id);
            if (subscription != null && !subscription.IsActive)
            {
                subscription.IsActive = true;
                subscription.DateChangeStatus = DateTime.UtcNow;
                _paymentRepository.UpdateSubscription(subscription);
            }
        }

        public async Task HandleExpiration(string id)
        {
            var subscription = _paymentRepository.GetSubscriptionById(id);
            if (subscription != null && subscription.IsActive)
            {
                subscription.IsActive = false;
                subscription.DateChangeStatus = DateTime.UtcNow;
                _paymentRepository.UpdateSubscription(subscription);
            }
        }
    }
}
