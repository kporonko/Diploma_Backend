using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Dto.Request
{
    public class PayPalSubscriptionRequestShort
    {
        public string ReturnUrl { get; set; }
        public string CancelUrl { get; set; }
    }
}
