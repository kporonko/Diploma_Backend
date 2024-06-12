﻿using Diploma.Backend.Application.Dto.Request;
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

        public async Task<BaseResponse<PayPalSubscriptionResponse>> CreateSubscription(PayPalSubscriptionRequestShort request, User jwtUser)
        {
            var existingSubscription = _paymentRepository.GetSubscriptionByUserId(jwtUser.Id);
            if (existingSubscription != null)
            {
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<PayPalSubscriptionResponse>(ErrorCodes.SubscriptionAlreadyExists.ToString());
            }
            var token = await _payPalProxy.GetTokenAsync();

            var product = await _payPalProxy.CreateProductAsync(token.access_token);
            var plan = await _payPalProxy.CreatePlanAsync(token.access_token, product.Id);
            var subscrResponse = await _payPalProxy.CreateSubscriptionAsync(request, token.access_token, plan.Id);
            var user = _paymentRepository.GetUserById(jwtUser.Id);

            if (user == null)
            {
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<PayPalSubscriptionResponse>(ErrorCodes.UserNotFound.ToString());
            }
            var subscription = new Subscription
            {
                User = user,
                UserId = user.Id,
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
            _paymentRepository.UpdateSubscription(subscription);

            return BaseResponseGenerator.GenerateValidBaseResponse(subscrResponse);
        }
      
        public async Task HandleExpiration(string id)
        {
            var subscription = _paymentRepository.GetSubscriptionById(id);
            if (subscription != null)
            {
                await _paymentRepository.DeleteSubscriptionAsync(subscription);
            }
        }
    }
}
