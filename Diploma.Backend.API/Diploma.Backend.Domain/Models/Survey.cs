using Diploma.Backend.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Domain.Models
{
    public class Survey : BaseEntity
    {
        public string Name { get; set; }
        public DateTime DateBy { get; set; }
        public string Params { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
        public int TargetingId { get; set; }
        public Targeting? Targeting { get; set; }
        public List<SurveyInUnit> SurveyInUnits { get; set; } = new List<SurveyInUnit>();
    }
}
