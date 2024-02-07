using Diploma.Backend.Domain.Common;
using Diploma.Backend.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Domain.Models
{
    public class UnitAppearance : BaseEntity
    {
        public AppearanceType Type { get; set; }
        public string Params { get; set; }
        public string Name { get; set; }
        public bool State { get; set; }

        public List<SurveyUnit> SurveyUnits { get; set; } = new List<SurveyUnit>();
        public int TemplateId { get; set; }
        public Template Template { get; set; }


        public int UserId { get; set; }
        public User User { get; set; }

    }
}
