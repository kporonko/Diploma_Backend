using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Application.Helpers;
using Diploma.Backend.Application.Mappers;
using Diploma.Backend.Domain.Common;
using Diploma.Backend.Domain.Enums;
using Diploma.Backend.Domain.Models;
using Diploma.Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Services.impl
{
    public class SurveyService : ISurveyService
    {
        private readonly ApplicationContext _context;
        private IConfiguration _config;

        public SurveyService(ApplicationContext context,
            IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<BaseResponse<SurveyResponse>> CreateSurvey(User userJwt, SurveyCreateRequest surveyCreateRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userJwt.Id);
            if (user == null)
                BaseResponseGenerator.GenerateBaseResponseByErrorMessage<SurveyResponse>(ErrorCodes.UserNotFound.ToString());

            var response = SurveyMapper.ConvertSurveyCreateRequestToSurvey(surveyCreateRequest, user);
            await AddSurveyToDb(response);
            return BaseResponseGenerator.GenerateValidBaseResponse(SurveyMapper.ConvertSurveyToSurveyResponse(response));
        }
        
        private async Task AddSurveyToDb(Survey response)
        {
            await _context.Surveys.AddAsync(response);
            await  _context.SaveChangesAsync();
        }
    }
}
