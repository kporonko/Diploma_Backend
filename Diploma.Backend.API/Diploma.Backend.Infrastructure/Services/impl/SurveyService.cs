using Azure;
using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Application.Helpers;
using Diploma.Backend.Application.Mappers;
using Diploma.Backend.Application.Services;
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

namespace Diploma.Backend.Infrastructure.Services.impl
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

        public async Task<BaseResponse<string>> DeleteSurvey(User userJwt, SurveyDeleteRequest surveyDeleteRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userJwt.Id);
            if (user == null)
                BaseResponseGenerator.GenerateBaseResponseByErrorMessage<SurveyUnitResponse>(ErrorCodes.UserNotFound.ToString());

            var survey = await _context.Surveys.FirstOrDefaultAsync(x => x.Id == surveyDeleteRequest.Id);

            await DeleteSurveyFromDb(survey);
            return BaseResponseGenerator.GenerateValidBaseResponse("Survey deleted");
        }

        public async Task<BaseResponse<SurveyResponse>> EditSurvey(User userJwt, SurveyEditRequest surveyEditRequest)
        {
            var user = await GetUserById(userJwt.Id);
            if (user == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<SurveyResponse>(ErrorCodes.UserNotFound.ToString());

            var survey = await GetSurveyByIdWithDetails(surveyEditRequest.Id);
            if (survey == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<SurveyResponse>(ErrorCodes.SurveyNotFound.ToString());

            UpdateSurveyProperties(survey, surveyEditRequest);
            RemoveExistingQuestionsAndOptions(survey);
            AddNewQuestionsAndOptions(survey, surveyEditRequest);

            await _context.SaveChangesAsync();

            var surveyResponse = SurveyMapper.ConvertSurveyToSurveyResponse(survey);
            return BaseResponseGenerator.GenerateValidBaseResponse(surveyResponse);
        }

        public async Task<BaseResponse<SurveyResponse>> GetSurveyById(User userJwt, int surveyId)
        {
            var user = await GetUserById(userJwt.Id);
            if (user == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<SurveyResponse>(ErrorCodes.UserNotFound.ToString());

            var survey = await GetSurveyByIdWithDetails(surveyId);
            if (survey == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<SurveyResponse>(ErrorCodes.SurveyNotFound.ToString());

            var surveyResponse = SurveyMapper.ConvertSurveyToSurveyResponse(survey);
            return BaseResponseGenerator.GenerateValidBaseResponse(surveyResponse);
        }

        public async Task<BaseResponse<List<SurveyShortResponse>>> GetUserSurveys(User userJwt)
        {
            var user = await GetUserById(userJwt.Id);
            if (user == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<List<SurveyShortResponse>>(ErrorCodes.UserNotFound.ToString());

            var surveys = await _context.Surveys
                .Include(x => x.Questions)
                .Where(s => s.UserId == user.Id)
                .ToListAsync();

            var surveyResponses = surveys.Select(survey => SurveyMapper.ConvertSurveyToSurveyShortResponse(survey)).ToList();
            return BaseResponseGenerator.GenerateValidBaseResponse(surveyResponses);
        }

        private async Task<User> GetUserById(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }

        private async Task<Survey> GetSurveyByIdWithDetails(int surveyId)
        {
            return await _context.Surveys
                .Include(s => s.Questions)
                .ThenInclude(q => q.QuestionOptions)
                .ThenInclude(o => o.OptionTranslations)
                .Include(s => s.Questions)
                .ThenInclude(q => q.QuestionLine)
                .ThenInclude(ql => ql.QuestionTranslations)
                .FirstOrDefaultAsync(x => x.Id == surveyId);
        }

        private void UpdateSurveyProperties(Survey survey, SurveyEditRequest surveyEditRequest)
        {
            survey.Name = surveyEditRequest.Name;
            survey.DateBy = surveyEditRequest.DateBy;
        }

        private void RemoveExistingQuestionsAndOptions(Survey survey)
        {
            _context.QuestionOptions.RemoveRange(survey.Questions.SelectMany(q => q.QuestionOptions));
            _context.Questions.RemoveRange(survey.Questions);
        }

        private void AddNewQuestionsAndOptions(Survey survey, SurveyEditRequest surveyEditRequest)
        {
            survey.Questions = surveyEditRequest.Questions.Select(questionRequest => CreateQuestionFromRequest(questionRequest)).ToList();
        }

        private Question CreateQuestionFromRequest(SurveyCreateRequestQuestion questionRequest)
        {
            return new Question
            {
                Type = (QuestionType)questionRequest.Type,
                OrderNumber = questionRequest.OrderNumber,
                QuestionLine = new QuestionLine
                {
                    QuestionTranslations = questionRequest.QuestionLine.Translations.Select(t => new QuestionTranslation
                    {
                        Language = t.LanguageCode,
                        QuestionTranslationLine = t.TranslationText
                    }).ToList()
                },
                QuestionOptions = questionRequest.QuestionOptions.Select(CreateOptionFromRequest).ToList()
            };
        }

        private QuestionOption CreateOptionFromRequest(SurveyCreateRequestQuestionOption optionRequest)
        {
            return new QuestionOption
            {
                OrderNumber = optionRequest.OrderNumber,
                OptionTranslations = optionRequest.Translations.Select(t => new OptionTranslation
                {
                    Language = t.LanguageCode,
                    OptionLine = t.TranslationText
                }).ToList()
            };
        }

        private async Task DeleteSurveyFromDb(Survey? survey)
        {
            _context.Surveys.Remove(survey);
            await _context.SaveChangesAsync();
        }

        private async Task AddSurveyToDb(Survey response)
        {
            await _context.Surveys.AddAsync(response);
            await  _context.SaveChangesAsync();
        }
    }
}
