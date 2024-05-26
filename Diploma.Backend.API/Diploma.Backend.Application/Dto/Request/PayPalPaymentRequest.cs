using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Dto.Request
{
    public class PayPalPaymentRequest
    {
        public string Note { get; set; }
        [JsonProperty("capture_type")]
        public string CaptureType { get; set; }
        public Amount Amount { get; set; }
    }

    public class Amount
    {
        [JsonProperty("currency_code")]
        public string CurrencyCode { get; set; }
        public string Value { get; set; }
    }
}
