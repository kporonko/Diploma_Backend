using Diploma.Backend.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Domain.Models
{
    public class SurveyUnit : BaseEntity
    {
        public string Name { get; set; }
        public int SettingsId { get; set; }
        public UnitSettings UnitSettings { get; set; }
        public int AppearanceId { get; set; }
        public UnitAppearance UnitAppearance { get; set; }

        public List<SurveyInUnit> SurveyInUnits { get; set; } = new List<SurveyInUnit>();
    }
}
