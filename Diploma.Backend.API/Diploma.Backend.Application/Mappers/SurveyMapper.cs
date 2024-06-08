using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Domain.Enums;
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
        public static Survey ConvertSurveyCreateRequestToSurvey(SurveyCreateRequest surveyCreateRequest, User user, Targeting targeting)
        {
            var survey = new Survey
            {
                DateBy = surveyCreateRequest.DateBy,
                Name = surveyCreateRequest.Name,
                User = user,
                UserId = user.Id,
                Params = "",
                TargetingId = surveyCreateRequest.TargetingId,
                Targeting = targeting
            };

            survey.Questions = ConvertQuestionRequestToQuestion(surveyCreateRequest.Questions, survey);

            return survey;
        }

        private static List<Question> ConvertQuestionRequestToQuestion(List<SurveyCreateRequestQuestion> questions, Survey survey)
        {
            var questionList = new List<Question>();
            foreach (var question in questions)
            {
                if (Enum.IsDefined(typeof(Domain.Enums.QuestionType), question.Type))
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
                else
                {
                    throw new Exception(ErrorCodes.QuestionTypeNotFound.ToString());
                }
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
                Questions = ConvertQuestionListToResponseList(response),
                Targeting = ConvertTargetingToResponse(response.Targeting)
            };
        }

        private static TargetingResponse ConvertTargetingToResponse(Targeting? targeting)
        {
            if (targeting == null)
            {
                return null;
            }
            return new TargetingResponse
            {
                UserId = targeting.UserId,
                Id = targeting.Id,
                Name = targeting.Name,
                CountryInTargetings = targeting.CountryInTargetings.Select(x => x.Country).ToDictionary(x => x.Id, x => x.Name)
            };
        }
        public static SurveyShortResponse ConvertSurveyToSurveyShortResponse(Survey response)
        {
            return new SurveyShortResponse
            {
                Id = response.Id,
                DateBy = response.DateBy,
                Name = response.Name,
                NumberOfQuestions = response.Questions.Count,
                Targeting = ConvertTargetingToResponse(response.Targeting)
            };
        }

        private static List<QuestionResponse> ConvertQuestionListToResponseList(Survey response)
        {
            var resList = new List<QuestionResponse>();
            foreach (var question in response.Questions)
            {
                resList.Add(new QuestionResponse
                {
                    Id = question.Id,
                    OrderNumber = question.OrderNumber,
                    SurveyId = response.Id,
                    QuestionLine = ConvertQuestionLineToResponse(question.QuestionLine),
                    QuestionOptions = ConvertQuestionOptionListToResponseList(question),
                    Type = question.Type
                });
            }

            return resList;
        }

        private static QuestionLineResponse ConvertQuestionLineToResponse(QuestionLine questionLine)
        {
            return new QuestionLineResponse
            {
                Id = questionLine.Id,
                QuestionId = questionLine.QuestionId,
                QuestionTranslations = ConvertQuestionTranslationToResponse(questionLine.QuestionTranslations)
            };
        }

        private static List<QuestionTranslationResponse> ConvertQuestionTranslationToResponse(List<QuestionTranslation> questionTranslations)
        {
            var resList = new List<QuestionTranslationResponse>();
            foreach (var translation in questionTranslations)
            {
                resList.Add(new QuestionTranslationResponse
                {
                    Id = translation.Id,
                    Language = translation.Language,
                    QuestionLineId = translation.QuestionLineId,
                    QuestionTranslationLine = translation.QuestionTranslationLine
                });
            }

            return resList;
        }

        private static List<QuestionOptionResponse> ConvertQuestionOptionListToResponseList(Question question)
        {
            var resList = new List<QuestionOptionResponse>();
            foreach (var option in question.QuestionOptions)
            {
                resList.Add(new QuestionOptionResponse
                {
                    Id = option.Id,
                    OrderNumber = option.OrderNumber,
                    QuestionId = question.Id,
                    OptionTranslations = ConvertOptionTranslationListToResponseList(option),
                });
            }
            return resList;
        }


        private static List<OptionTranslationResponse> ConvertOptionTranslationListToResponseList(QuestionOption option)
        {
            var resList = new List<OptionTranslationResponse>();
            foreach (var translation in option.OptionTranslations)
            {
                resList.Add(new OptionTranslationResponse
                {
                    Id = translation.Id,
                    Language = translation.Language,
                    OptionLine = translation.OptionLine,
                    QuestionOptionId = option.Id
                });
            }
            return resList;
        }

        public static SurveyIdResponse ConvertSurveyToSurveyIdResponse(Survey response)
        {
            return new SurveyIdResponse
            {
                Id = response.Id
            };
        }
    }
}
