using Diploma.Backend.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Domain.Models
{
    public class Country : BaseEntity
    {
        public string Name { get; set; }

        public List<CountryInTargeting> CountryInTargetings { get; set; } = new List<CountryInTargeting>();
    }
}
