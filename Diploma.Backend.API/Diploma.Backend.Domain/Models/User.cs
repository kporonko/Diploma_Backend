using Diploma.Backend.Domain.Common;
using Diploma.Backend.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Domain.Models
{
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        public List<Survey> Surveys { get; set; } = new List<Survey>();
        public List<Targeting> Targetings { get; set; } = new List<Targeting>();
        public List<SurveyUnit> SurveyUnits { get; set; } = new List<SurveyUnit>();
        public List<UnitAppearance> UnitAppearances { get; set; } = new List<UnitAppearance>();
        public List<UnitSettings> UnitSettings { get; set; } = new List<UnitSettings>();
        public Subscription? Subscription { get; set; }
    }
}
