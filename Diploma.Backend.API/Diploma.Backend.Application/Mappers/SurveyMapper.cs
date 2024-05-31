using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Mappers
{
    public static class SurveyMapper
    {
        // not sure it works in the way that we try to assign id of entity that is not being set yet.
        public static Survey ConvertSurveyCreateRequestToSurvey(SurveyCreateRequest surveyCreateRequest, User user)
        {
            // params prop?
            // survey in unit prop?
            // targeting prop?
            var survey = new Survey
            {
                DateBy = surveyCreateRequest.DateBy,
                Name = surveyCreateRequest.Name,
                User = user,
                UserId = user.Id,
                Params = "",
                TargetingId = 1
            };

            survey.Questions = ConvertQuestionRequestToQuestion(surveyCreateRequest.Questions, survey);

            return survey;
        }

        private static List<Question> ConvertQuestionRequestToQuestion(List<SurveyCreateRequestQuestion> questions, Survey survey)
        {
            var questionList = new List<Question>();
            foreach (var question in questions)
            {
                var newQuestion = new Question
                {
                    OrderNumber = question.OrderNumber,
                    SurveyId = survey.Id,
                    Type = (Domain.Enums.QuestionType)question.Type
                };
                newQuestion.QuestionLine = ConvertQuestionLineRequestToQuestionLine(newQuestion, question);
                newQuestion.QuestionOptions = ConvertQuestionOptionsRequestToQuestionOptions(newQuestion, question);

                questionList.Add(newQuestion);
            }
            return questionList;
        }

        private static List<QuestionOption> ConvertQuestionOptionsRequestToQuestionOptions(Question q, SurveyCreateRequestQuestion question)
        {
            var resList = new List<QuestionOption>();
            foreach (var option in question.QuestionOptions)
            {
                var currOption = new QuestionOption
                {
                    OrderNumber = option.OrderNumber,
                    Question = q
                };
                
                currOption.OptionTranslations = ConvertTranslations(option.Translations, currOption);
                resList.Add(currOption);
            }

            return resList;
        }

        private static List<OptionTranslation> ConvertTranslations(List<SurveyCreateRequestTranslation> translations, QuestionOption option)
        {
            var resList = new List<OptionTranslation>();
            foreach (var translation in translations)
            {
                resList.Add(new OptionTranslation
                {
                    Language = translation.LanguageCode,
                    OptionLine = translation.TranslationText,
                    QuestionOptionId = option.Id,
                });
            }
            return resList;
        }

        private static QuestionLine ConvertQuestionLineRequestToQuestionLine(Question question, SurveyCreateRequestQuestion surveyCreateRequestQuestion)
        {
            var questionLine = new QuestionLine
            {
                Question = question,
            };
            questionLine.QuestionTranslations = surveyCreateRequestQuestion.QuestionLine.Translations.Select(x => new QuestionTranslation
            {
                Language = x.LanguageCode,
                QuestionLine = questionLine,
                QuestionTranslationLine = x.TranslationText
            }).ToList();
            
            return questionLine;
        }


        public static SurveyResponse ConvertSurveyToSurveyResponse(Survey response)
        {
            return new SurveyResponse
            {
                Id = response.Id,
                DateBy = response.DateBy,
                Name = response.Name,
                Questions = ConvertQuestionListToResponseList(response)
            };
        }

        public static SurveyShortResponse ConvertSurveyToSurveyShortResponse(Survey response)
        {
            return new SurveyShortResponse
            {
                Id = response.Id,
                DateBy = response.DateBy,
                Name = response.Name,
                NumberOfQuestions = response.Questions.Count
            };
        }

        private static List<Question> ConvertQuestionListToResponseList(Survey response)
        {
            var resList = new List<Question>();
            foreach (var question in response.Questions)
            {
                resList.Add(new Question
                {
                    Id = question.Id,
                    OrderNumber = question.OrderNumber,
                    SurveyId = response.Id,
                    QuestionLine = question.QuestionLine,
                    QuestionOptions = ConvertQuestionOptionListToResponseList(question),
                });
            }

            return resList;
        }

        private static List<QuestionOption> ConvertQuestionOptionListToResponseList(Question question)
        {
            var resList = new List<QuestionOption>();
            foreach (var option in question.QuestionOptions)
            {
                resList.Add(new QuestionOption
                {
                    Id = option.Id,
                    OrderNumber = option.OrderNumber,
                    QuestionId = question.Id,
                    OptionTranslations = ConvertOptionTranslationListToResponseList(option),
                });
            }
            return resList;
        }


        private static List<OptionTranslation> ConvertOptionTranslationListToResponseList(QuestionOption option)
        {
            var resList = new List<OptionTranslation>();
            foreach (var translation in option.OptionTranslations)
            {
                resList.Add(new OptionTranslation
                {
                    Id = translation.Id,
                    Language = translation.Language,
                    OptionLine = translation.OptionLine,
                    QuestionOptionId = option.Id,
                });
            }
            return resList;
        }
    }
}
