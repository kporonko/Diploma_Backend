using Diploma.Backend.Application.Dto.Request;
using Microsoft.Extensions.Configuration;

namespace Diploma.Backend.Infrastructure.PayPal.Models
{
    public class PayPalRequestDataConfig
    {
        public PayPalProductRequest Product { get; }
        public PayPalPlanRequest Plan { get; }
        public PayPalSubscriptionRequest Subscription { get; }

        public PayPalRequestDataConfig(IConfiguration configuration)
        {
            var payPalSection = configuration.GetSection("PayPalRequestDataConfig");

            // Product Configuration
            var productSection = payPalSection.GetSection("Product");
            Product = new PayPalProductRequest
            {
                Name = productSection.GetSection("Name").Value,
                Type = productSection.GetSection("Type").Value
            };

            // Plan Configuration
            var planSection = payPalSection.GetSection("Plan");
            Plan = new PayPalPlanRequest
            {
                Name = planSection.GetSection("Name").Value,
                BillingCycles = LoadBillingCycles(planSection.GetSection("BillingCycles")),
                PaymentPreferences = LoadPaymentPreferences(planSection.GetSection("PaymentPreferences")),
                Description = planSection.GetSection("Description").Value,
                Status = planSection.GetSection("Status").Value,
                Taxes = LoadTaxes(planSection.GetSection("Taxes"))
            };

            // Subscription Configuration
            var subscriptionSection = payPalSection.GetSection("Subscription");
            Subscription = new PayPalSubscriptionRequest
            {
                shipping_amount = LoadShippingAmount(subscriptionSection.GetSection("ShippingAmount")),
                subscriber = LoadSubscriber(subscriptionSection.GetSection("Subscriber")),
                application_context = LoadApplicationContext(subscriptionSection.GetSection("ApplicationContext"))
            };
        }

        private List<BillingCycle> LoadBillingCycles(IConfigurationSection section)
        {
            var billingCycles = new List<BillingCycle>();
            foreach (var bcSection in section.GetChildren())
            {
                var billingCycle = new BillingCycle
                {
                    TenureType = bcSection.GetSection("TenureType").Value,
                    Sequence = int.Parse(bcSection.GetSection("Sequence").Value),
                    Frequency = new Frequency
                    {
                        IntervalUnit = bcSection.GetSection("Frequency").GetSection("IntervalUnit").Value,
                        IntervalCount = int.Parse(bcSection.GetSection("Frequency").GetSection("IntervalCount").Value)
                    },
                    TotalCycles = int.Parse(bcSection.GetSection("TotalCycles").Value),
                    PricingScheme = new PricingScheme
                    {
                        FixedPrice = new FixedPrice
                        {
                            Value = bcSection.GetSection("PricingScheme").GetSection("FixedPrice").GetSection("Value").Value,
                            CurrencyCode = bcSection.GetSection("PricingScheme").GetSection("FixedPrice").GetSection("CurrencyCode").Value
                        }
                    }
                };
                billingCycles.Add(billingCycle);
            }
            return billingCycles;
        }

        private PaymentPreferences LoadPaymentPreferences(IConfigurationSection section)
        {
            return new PaymentPreferences
            {
                AutoBillOutstanding = bool.Parse(section.GetSection("AutoBillOutstanding").Value),
                SetupFee = new SetupFee
                {
                    Value = section.GetSection("SetupFee").GetSection("Value").Value,
                    CurrencyCode = section.GetSection("SetupFee").GetSection("CurrencyCode").Value
                },
                SetupFeeFailureAction = section.GetSection("SetupFeeFailureAction").Value,
                PaymentFailureThreshold = int.Parse(section.GetSection("PaymentFailureThreshold").Value)
            };
        }

        private Taxes LoadTaxes(IConfigurationSection section)
        {
            return new Taxes
            {
                Percentage = section.GetSection("Percentage").Value,
                Inclusive = bool.Parse(section.GetSection("Inclusive").Value)
            };
        }

        private PayPalShippingAmount LoadShippingAmount(IConfigurationSection section)
        {
            return new PayPalShippingAmount
            {
                currency_code = section.GetSection("CurrencyCode").Value,
                value = section.GetSection("Value").Value
            };
        }

        private PayPalSubscriber LoadSubscriber(IConfigurationSection section)
        {
            return new PayPalSubscriber
            {
                name = new PayPalName
                {
                    given_name = section.GetSection("Name").GetSection("GivenName").Value,
                    surname = section.GetSection("Name").GetSection("Surname").Value
                },
                email_address = section.GetSection("EmailAddress").Value,
                shipping_address = new PayPalShippingAddress
                {
                    name = new PayPalNameFull
                    {
                        full_name = section.GetSection("ShippingAddress").GetSection("Name").GetSection("FullName").Value
                    },
                    address = new PayPalAddress
                    {
                        address_line_1 = section.GetSection("ShippingAddress").GetSection("Address").GetSection("AddressLine1").Value,
                        address_line_2 = section.GetSection("ShippingAddress").GetSection("Address").GetSection("AddressLine2").Value,
                        admin_area_2 = section.GetSection("ShippingAddress").GetSection("Address").GetSection("AdminArea2").Value,
                        admin_area_1 = section.GetSection("ShippingAddress").GetSection("Address").GetSection("AdminArea1").Value,
                        postal_code = section.GetSection("ShippingAddress").GetSection("Address").GetSection("PostalCode").Value,
                        country_code = section.GetSection("ShippingAddress").GetSection("Address").GetSection("CountryCode").Value
                    }
                }
            };
        }

        private PayPalApplicationContext LoadApplicationContext(IConfigurationSection section)
        {
            return new PayPalApplicationContext
            {
                brand_name = section.GetSection("BrandName").Value,
                locale = section.GetSection("Locale").Value,
                shipping_preference = section.GetSection("ShippingPreference").Value,
                user_action = section.GetSection("UserAction").Value,
                payment_method = new PayPalPaymentMethod
                {
                    payer_selected = section.GetSection("PaymentMethod").GetSection("PayerSelected").Value,
                    payee_preferred = section.GetSection("PaymentMethod").GetSection("PayeePreferred").Value
                },
            };
        }
    }
}