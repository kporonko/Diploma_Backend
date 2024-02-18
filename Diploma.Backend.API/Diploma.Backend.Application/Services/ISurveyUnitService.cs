using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Domain.Common;
using Diploma.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Services
{
    public interface ISurveyUnitService
    {
        Task<BaseResponse<SurveyUnitResponse>> CreateSurveyUnit(User userJwt, SurveyUnitCreateRequest surveyUnitCreateRequest);
        Task<BaseResponse<SurveyUnitResponse>> EditSurveyUnit(User userJwt, SurveyUnitEditRequest surveyUnitCreateRequest);
        Task<BaseResponse<string>> DeleteSurveyUnit(User userJwt, SurveyUnitDeleteRequest surveyUnitCreateRequest);
        Task<BaseResponse<List<SurveyUnitResponse>>> GetSurveyUnits(User userJwt);
    }
}
