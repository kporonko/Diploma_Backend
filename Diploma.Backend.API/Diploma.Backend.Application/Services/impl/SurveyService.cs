using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Application.Helpers;
using Diploma.Backend.Application.Mappers;
using Diploma.Backend.Application.Repositories;
using Diploma.Backend.Application.Services;
using Diploma.Backend.Domain.Common;
using Diploma.Backend.Domain.Enums;
using Diploma.Backend.Domain.Models;
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
        private readonly ISurveyRepository _surveyRepository;

        public SurveyService(ISurveyRepository surveyRepository)
        {
            _surveyRepository = surveyRepository;
        }

        public async Task<BaseResponse<SurveyResponse>> CreateSurvey(User userJwt, SurveyCreateRequest surveyCreateRequest)
        {
            var user = await _surveyRepository.GetUserByIdAsync(userJwt.Id);
            if (user == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<SurveyResponse>(ErrorCodes.UserNotFound.ToString());

            var response = SurveyMapper.ConvertSurveyCreateRequestToSurvey(surveyCreateRequest, user);
            await _surveyRepository.AddSurveyAsync(response);
            return BaseResponseGenerator.GenerateValidBaseResponse(SurveyMapper.ConvertSurveyToSurveyResponse(response));
        }

        public async Task<BaseResponse<string>> DeleteSurvey(User userJwt, SurveyDeleteRequest surveyDeleteRequest)
        {
            var user = await _surveyRepository.GetUserByIdAsync(userJwt.Id);
            if (user == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<string>(ErrorCodes.UserNotFound.ToString());

            await _surveyRepository.DeleteSurveyAsync(surveyDeleteRequest.Id);
            return BaseResponseGenerator.GenerateValidBaseResponse("Survey deleted");
        }

        public async Task<BaseResponse<SurveyResponse>> EditSurvey(User userJwt, SurveyEditRequest surveyEditRequest)
        {
            var user = await _surveyRepository.GetUserByIdAsync(userJwt.Id);
            if (user == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<SurveyResponse>(ErrorCodes.UserNotFound.ToString());

            var survey = await _surveyRepository.GetSurveyByIdWithDetailsAsync(surveyEditRequest.Id);
            if (survey == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<SurveyResponse>(ErrorCodes.SurveyNotFound.ToString());

            UpdateSurveyProperties(survey, surveyEditRequest);
            RemoveExistingQuestionsAndOptions(survey);
            if (surveyEditRequest.Questions != null)
            {
                AddNewQuestionsAndOptions(survey, surveyEditRequest);
            }

            await _surveyRepository.UpdateSurveyAsync(survey);

            var surveyResponse = SurveyMapper.ConvertSurveyToSurveyResponse(survey);
            return BaseResponseGenerator.GenerateValidBaseResponse(surveyResponse);
        }

        public async Task<BaseResponse<SurveyResponse>> GetSurveyById(User userJwt, int surveyId)
        {
            var user = await _surveyRepository.GetUserByIdAsync(userJwt.Id);
            if (user == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<SurveyResponse>(ErrorCodes.UserNotFound.ToString());

            var survey = await _surveyRepository.GetSurveyByIdWithDetailsAsync(surveyId);
            if (survey == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<SurveyResponse>(ErrorCodes.SurveyNotFound.ToString());

            var surveyResponse = SurveyMapper.ConvertSurveyToSurveyResponse(survey);
            return BaseResponseGenerator.GenerateValidBaseResponse(surveyResponse);
        }

        public async Task<BaseResponse<List<SurveyShortResponse>>> GetUserSurveys(User userJwt)
        {
            var user = await _surveyRepository.GetUserByIdAsync(userJwt.Id);
            if (user == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<List<SurveyShortResponse>>(ErrorCodes.UserNotFound.ToString());

            var surveys = await _surveyRepository.GetUserSurveysAsync(user.Id);
            if (surveys == null)
                return BaseResponseGenerator.GenerateBaseResponseByErrorMessage<List<SurveyShortResponse>>(ErrorCodes.SurveyNotFound.ToString());

            var surveyResponses = surveys.Select(survey => SurveyMapper.ConvertSurveyToSurveyShortResponse(survey)).ToList();
            return BaseResponseGenerator.GenerateValidBaseResponse(surveyResponses);
        }

        private void UpdateSurveyProperties(Survey survey, SurveyEditRequest surveyEditRequest)
        {
            survey.Name = surveyEditRequest.Name;
            survey.DateBy = surveyEditRequest.DateBy;
        }

        private void RemoveExistingQuestionsAndOptions(Survey survey)
        {
            _surveyRepository.DeleteQuestionsAndOptions(survey);
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
    }
}
