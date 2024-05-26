using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Domain.Models
{
    public class Subscription
    {
        public int Id { get; set; }
        public string SubscriptionId { get; set; }
        public string Name { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateChangeStatus { get; set; }
        public string CurrencyCode { get; set; }
        public bool IsActive { get; set; }
        public decimal Price { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }

    }
}
