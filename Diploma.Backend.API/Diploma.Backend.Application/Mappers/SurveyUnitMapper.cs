using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Mappers
{
    public static class SurveyUnitMapper
    {
        public static SurveyUnitResponse MapSurveyUnitToResponse(SurveyUnit model, List<Survey> surveys)
        {
            return new SurveyUnitResponse
            {
                Id = model.Id,
                AppearanceIdName = new Dictionary<int, string>
                {
                    { model.UnitAppearance.Id, model.UnitAppearance.Name }
                },
                HideAfterNoSurveys = model.UnitSettings.HideAfterNoSurveys,
                MaximumSurveysPerDevice = model.UnitSettings.MaximumSurveysPerDevice,
                MessageAfterNoSurveys = model.UnitSettings.MessageAfterNoSurveys,
                Name = model.Name,
                OneSurveyTakePerDevice = model.UnitSettings.OneSurveyTakePerDevice,
                SurveyIdNames = surveys.ToDictionary(x => x.Id, x => x.Name)
            };
        }
    }
}
