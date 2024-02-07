using Diploma.Backend.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Domain.Models
{
    public class Targeting : BaseEntity
    {
        public string Name { get; set; }
        public List<CountryInTargeting> CountryInTargetings { get; set; } = new List<CountryInTargeting>();
        public List<Survey> Surveys { get; set; } = new List<Survey>();
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
