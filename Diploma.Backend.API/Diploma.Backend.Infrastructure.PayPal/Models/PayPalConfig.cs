using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Infrastructure.PayPal.Models
{
    public class PayPalConfig
    {
        public PayPalConfig(IConfiguration configuration)
        {
            var payPalSection = configuration.GetSection("PayPal");
            TimeOut = Convert.ToInt32(payPalSection.GetSection("TimeOut").Value);
            ClientId = payPalSection.GetSection("ClientId").Value;
            ClientSecret = payPalSection.GetSection("ClientSecret").Value;
            BaseUrl = payPalSection.GetSection("BaseUrl").Value;
            TokenUrl = payPalSection.GetSection("TokenUrl").Value;
            CreateSubscriptionUrl = payPalSection.GetSection("CreateSubscriptionUrl").Value;
            GetSubscriptionUrl = payPalSection.GetSection("GetSubscriptionUrl").Value;
            CancelSubscriptionUrl = payPalSection.GetSection("CancelSubscriptionUrl").Value;
            CreatePlanUrl = payPalSection.GetSection("CreatePlanUrl").Value;
            CapturePaymentUrl = payPalSection.GetSection("CapturePaymentUrl").Value;
            CreateProductUrl = payPalSection.GetSection("CreateProductUrl").Value;
            ActivateSubscriptionUrl = payPalSection.GetSection("ActivateSubscriptionUrl").Value;
        }
        
        public int TimeOut { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string BaseUrl { get; set; }
        public string TokenUrl { get; set; }
        public string CreateSubscriptionUrl { get; set; }
        public string GetSubscriptionUrl { get; set; }
        public string CancelSubscriptionUrl { get; set; }
        public string CreatePlanUrl { get; set; }
        public string CapturePaymentUrl { get; set; }
        public string CreateProductUrl { get; set; }
        public string ActivateSubscriptionUrl { get; set; }

    }
}
