using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Mappers
{
    public static class TargetingMapper
    { 
        public static Targeting ConvertTargetingCreateRequestToTargeting(TargetingCreateRequest targetingCreateRequest, int userId)
        {
            return new Targeting
            {
                Name = targetingCreateRequest.Name,
                UserId = userId           
            };
        }

        public static TargetingCreateResponse MapTargetingToResponse(Targeting targeting)
        {
            return new TargetingCreateResponse
            {
                Id = targeting.Id,
                Name = targeting.Name,
                Countries = targeting.CountryInTargetings.ToDictionary(
                    c => c.CountryId,
                    c => c.Country.Name
                ),
                SurveysIds = !targeting.Surveys.IsNullOrEmpty() ? targeting.Surveys.Select(s => s.Id).ToList() : null
            };
        }
    }
}
