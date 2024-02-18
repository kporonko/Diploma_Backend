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
    public interface ISurveyService
    {
        Task<BaseResponse<SurveyResponse>> CreateSurvey(User userJwt, SurveyCreateRequest surveyCreateRequest);
    }
}
