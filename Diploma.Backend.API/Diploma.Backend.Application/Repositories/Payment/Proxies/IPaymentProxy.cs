using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Dto.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Repositories.Payment.Proxies
{
    public interface IPaymentProxy
    {
        Task<PayPalAccessTokenResponse> GetTokenAsync();
        Task<PayPalSubscriptionResponse> CreateSubscriptionAsync(PayPalSubscriptionRequestShort request, string token, string planId);
        Task<PayPalSubscriptionResponse> GetSubscriptionAsync(string id, string token);
        Task<PayPalCancelSubscriptionResponse> CancelSubscriptionAsync(string id, PayPalCancelSubscriptionRequest request, string token);
        Task<PayPalSubscriptionResponse> ActivateSubscriptionAsync(string id, ActivateSubscriptionRequest request, string token);
        Task<PayPalPlanResponse> CreatePlanAsync(string token, string productId);
        Task<PayPalProductResponse> CreateProductAsync(string token);
    }
}
