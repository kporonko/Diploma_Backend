using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Dto.Response
{
    public class PayPalSubscriptionResponse
    {
        public string id { get; set; }
        public string status { get; set; }
        public DateTime status_update_time { get; set; }
        public string plan_id { get; set; }
        public bool plan_overridden { get; set; }
        public DateTime start_time { get; set; }
        public string quantity { get; set; }
        public PayPalShippingAmount shipping_amount { get; set; }
        public PayPalSubscriber subscriber { get; set; }
        public DateTime create_time { get; set; }
        public List<PayPalLink> links { get; set; }
    }

    public class PayPalShippingAmount
    {
        public string currency_code { get; set; }
        public string value { get; set; }
    }

    public class PayPalSubscriber
    {
        public PayPalName name { get; set; }
        public string email_address { get; set; }
        public string payer_id { get; set; }
        public PayPalShippingAddress shipping_address { get; set; }
    }

    public class PayPalName
    {
        public string given_name { get; set; }
        public string surname { get; set; }
    }

    public class PayPalShippingAddress
    {
        public PayPalName name { get; set; }
        public PayPalAddress address { get; set; }
    }

    public class PayPalAddress
    {
        public string address_line_1 { get; set; }
        public string address_line_2 { get; set; }
        public string admin_area_2 { get; set; }
        public string admin_area_1 { get; set; }
        public string postal_code { get; set; }
        public string country_code { get; set; }
    }

    public class PayPalLink
    {
        public string href { get; set; }
        public string rel { get; set; }
        public string method { get; set; }
    }
}
