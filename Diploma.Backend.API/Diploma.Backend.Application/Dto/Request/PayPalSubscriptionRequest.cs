using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Dto.Request
{
    public class PayPalSubscriptionRequest
    {
        public string plan_id { get; set; }
        public DateTime start_time { get; set; }
        //public string quantity { get; set; }
        public PayPalShippingAmount shipping_amount { get; set; }
        public PayPalSubscriber subscriber { get; set; }
        public PayPalApplicationContext application_context { get; set; }
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
        public PayPalShippingAddress shipping_address { get; set; }
    }

    public class PayPalName
    {
        public string given_name { get; set; }
        public string surname { get; set; }
    }

    public class PayPalNameFull
    {
        public string full_name { get; set; }
    }

    
    public class PayPalShippingAddress
    {
        public PayPalNameFull name { get; set; }
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

    public class PayPalApplicationContext
    {
        public string brand_name { get; set; }
        public string locale { get; set; }
        public string shipping_preference { get; set; }
        public string user_action { get; set; }
        public PayPalPaymentMethod payment_method { get; set; }
        public string return_url { get; set; }
        public string cancel_url { get; set; }
    }

    public class PayPalPaymentMethod
    {
        public string payer_selected { get; set; }
        public string payee_preferred { get; set; }
    }
}
