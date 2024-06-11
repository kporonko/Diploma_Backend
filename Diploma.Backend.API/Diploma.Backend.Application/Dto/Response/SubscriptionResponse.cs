using Diploma.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Dto.Response
{
    public class SubscriptionResponse
    {
        public int Id { get; set; }
        public string SubscriptionId { get; set; }
        public int UserId { get; set; }
    }
}
