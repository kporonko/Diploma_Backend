using Diploma.Backend.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Domain.Models
{
    public class CountryInTargeting : BaseEntity
    {
        public int CountryId { get; set; }
        public Country Country { get; set; }

        public int TargetingId { get; set; }
        public Targeting Targeting { get; set; }
    }
}
