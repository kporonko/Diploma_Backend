
using Diploma.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Dto.Response
{
    public class SurveyUnitResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OneSurveyTakePerDevice { get; set; }
        public int MaximumSurveysPerDevice { get; set; }
        public bool HideAfterNoSurveys { get; set; }
        public bool MessageAfterNoSurveys { get; set; }
        public Dictionary<int, string> SurveyIdNames { get; set; }
        public Dictionary<int, string> AppearanceIdName { get; set; }
    }
}
