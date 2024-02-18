using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Dto.Request
{
    public class SurveyUnitEditRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OneSurveyTakePerDevice { get; set; }
        public int MaximumSurveysPerDevice { get; set; }
        public bool HideAfterNoSurveys { get; set; }
        public bool MessageAfterNoSurveys { get; set; }
        public List<int> SurveyIds { get; set; }
        public int AppearanceId { get; set; }
    }
}
