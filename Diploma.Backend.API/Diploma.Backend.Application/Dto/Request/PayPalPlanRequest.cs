using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Dto.Request
{
    public class PayPalPlanRequest
    {
        [JsonProperty("product_id")]
        public string ProductId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("billing_cycles")]
        public List<BillingCycle> BillingCycles { get; set; }

        [JsonProperty("payment_preferences")]
        public PaymentPreferences PaymentPreferences { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("taxes")]
        public Taxes Taxes { get; set; }
    }

    public class BillingCycle
    {
        [JsonProperty("tenure_type")]
        public string TenureType { get; set; }

        [JsonProperty("sequence")]
        public int Sequence { get; set; }

        [JsonProperty("frequency")]
        public Frequency Frequency { get; set; }

        [JsonProperty("total_cycles")]
        public int TotalCycles { get; set; }

        [JsonProperty("pricing_scheme")]
        public PricingScheme PricingScheme { get; set; }
    }

    public class Frequency
    {
        [JsonProperty("interval_unit")]
        public string IntervalUnit { get; set; }

        [JsonProperty("interval_count")]
        public int IntervalCount { get; set; }
    }

    public class PricingScheme
    {
        [JsonProperty("fixed_price")]
        public FixedPrice FixedPrice { get; set; }
    }

    public class FixedPrice
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("currency_code")]
        public string CurrencyCode { get; set; }
    }

    public class PaymentPreferences
    {
        [JsonProperty("auto_bill_outstanding")]
        public bool AutoBillOutstanding { get; set; }

        [JsonProperty("setup_fee")]
        public SetupFee SetupFee { get; set; }

        [JsonProperty("setup_fee_failure_action")]
        public string SetupFeeFailureAction { get; set; }

        [JsonProperty("payment_failure_threshold")]
        public int PaymentFailureThreshold { get; set; }
    }

    public class SetupFee
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("currency_code")]
        public string CurrencyCode { get; set; }
    }

    public class Taxes
    {
        [JsonProperty("percentage")]
        public string Percentage { get; set; }

        [JsonProperty("inclusive")]
        public bool Inclusive { get; set; }
    }
}
