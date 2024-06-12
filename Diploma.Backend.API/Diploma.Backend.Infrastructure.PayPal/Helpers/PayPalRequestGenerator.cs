using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Infrastructure.PayPal.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Infrastructure.PayPal.Helpers
{
    public class PayPalRequestGenerator
    {
        private PayPalRequestDataConfig _configuration;

        public PayPalRequestGenerator(IConfiguration configuration)
        {
            _configuration = new PayPalRequestDataConfig(configuration);
        }
        public PayPalProductRequest GenerateProductRequest()
        {
            return _configuration.Product;
        }

        public PayPalPlanRequest GeneratePlanRequest()
        {
            return _configuration.Plan;
        }

        public PayPalSubscriptionRequest GenerateSubscriptionRequest()
        {
            return _configuration.Subscription;
        }
    }
}
