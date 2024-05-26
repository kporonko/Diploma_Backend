using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Dto.Response
{
    public class PayPalPlanResponse
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public List<BillingCycle> BillingCycles { get; set; }
        public PaymentPreferences PaymentPreferences { get; set; }
        public Taxes Taxes { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public List<Link> Links { get; set; }
    }

    public class BillingCycle
    {
        public Frequency Frequency { get; set; }
        public string TenureType { get; set; }
        public int Sequence { get; set; }
        public int TotalCycles { get; set; }
        public PricingScheme PricingScheme { get; set; }
    }

    public class Frequency
    {
        public string IntervalUnit { get; set; }
        public int IntervalCount { get; set; }
    }

    public class PricingScheme
    {
        public FixedPrice FixedPrice { get; set; }
        public int Version { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }

    public class FixedPrice
    {
        public string CurrencyCode { get; set; }
        public string Value { get; set; }
    }

    public class PaymentPreferences
    {
        public bool AutoBillOutstanding { get; set; }
        public SetupFee SetupFee { get; set; }
        public string SetupFeeFailureAction { get; set; }
        public int PaymentFailureThreshold { get; set; }
    }

    public class SetupFee
    {
        public string Value { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class Taxes
    {
        public string Percentage { get; set; }
        public bool Inclusive { get; set; }
    }

    public class Link
    {
        public string Href { get; set; }
        public string Rel { get; set; }
        public string Method { get; set; }
    }
}
