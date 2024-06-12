using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Domain.Common;
using Diploma.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Services
{
    public interface IPaymentService
    {
        Task<BaseResponse<PayPalSubscriptionResponse>> GetSubscription(string id);
        Task<BaseResponse<PayPalSubscriptionResponse>> CreateSubscription(PayPalSubscriptionRequestShort request, User jwtUser);
        Task<BaseResponse<PayPalCancelSubscriptionResponse>> CancelSubscription(string id, PayPalCancelSubscriptionRequest request);
        Task<BaseResponse<PayPalSubscriptionResponse>> ActivateSubscription(string id, ActivateSubscriptionRequest request);

        Task HandleExpiration(string id);

    }
}
