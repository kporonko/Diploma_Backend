using Diploma.Backend.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Domain.Models
{
    public class SurveyInUnit : BaseEntity
    {
        public int SurveyId { get; set; }
        public Survey Survey { get; set; }
        public int SurveyUnitId { get; set; }
        public SurveyUnit SurveyUnit { get; set; }
    }
}
