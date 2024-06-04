using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Repositories;
using Diploma.Backend.Application.Services.impl;
using Diploma.Backend.Domain.Enums;
using Diploma.Backend.Domain.Models;
using Diploma.Backend.Infrastructure.Repositories.impl;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Tests.Integration.Services
{
    [TestFixture]
    public class SurveyServiceTests
    {
        private SurveyService _surveyService;
        private ISurveyRepository _survRepository;

        private readonly int SURVEY_ID_TO_GET = 1;
        private readonly int USER_ID_TO_GET = 3;
        private readonly int EXPECTED_QUESTIONS_COUNT = 1;
        private readonly int EXPECTED_USER_SURVEYS_COUNT = 23;
        private readonly int TARGETING_ID_TO_CREATE_SURVEY = 1;

        [SetUp]
        public void Setup()
        {
            _survRepository = new SurveyRepository(new Infrastructure.Data.ApplicationContext());

            _surveyService = new SurveyService(_survRepository);
        }

        [Test]
        public async Task GetSurveyById_ValidSurveyId_ReturnsSurvey()
        {
            // Arrange
            var surveyId = SURVEY_ID_TO_GET;
            var user = new User
            {
                Id = USER_ID_TO_GET,
            };
            // Act
            var response = await _surveyService.GetSurveyById(user, surveyId);

            // Assert
            Assert.IsNotNull(response.Data);
            Assert.IsNull(response.Error);
            Assert.AreEqual(surveyId, response.Data.Id);
            Assert.AreEqual(EXPECTED_QUESTIONS_COUNT, response.Data.Questions.Count);   
        }

        [Test]
        public async Task GetSurveysUser_UserWithSurveys_ReturnsResponse()
        {
            // Arrange
            var user = new User
            {
                Id = USER_ID_TO_GET,
            };
            
            // Act
            var response = await _surveyService.GetUserSurveys(user);

            // Assert
            Assert.IsNotNull(response.Data);
            Assert.IsNull(response.Error);
            Assert.AreEqual(EXPECTED_USER_SURVEYS_COUNT, response.Data.Count);

        }

        [Test]
        public async Task CreateSurvey_ValidData_ReturnsCorrectResponse()
        {
            // Arrange
            var request = GetCreateSurveyRequest();
            var user = new User
            {
                Id = USER_ID_TO_GET,
            };
            // Act
            var response = await _surveyService.CreateSurvey(user, request);

            // Assert
            Assert.IsNotNull(response.Data);
            Assert.IsNull(response.Error);
            Assert.AreEqual(TARGETING_ID_TO_CREATE_SURVEY, response.Data.Targeting.Id);
            Assert.AreEqual(request.Name, response.Data.Name);
            Assert.AreEqual(request.Questions.Count, response.Data.Questions.Count);
            Assert.AreEqual(request.DateBy, response.Data.DateBy);
            Assert.AreEqual((QuestionType)request.Questions.First().Type, response.Data.Questions.First().Type);
        }

        private SurveyCreateRequest GetCreateSurveyRequest()
        {
            return new SurveyCreateRequest
            {
                DateBy = DateTime.Now,
                Name = "Test Survey",
                TargetingId = 1,
                Questions = new List<SurveyCreateRequestQuestion>
                {
                    new SurveyCreateRequestQuestion
                    {
                        Id = 1,
                        OrderNumber = 1,
                        QuestionLine = new SurveyCreateRequestQuestionLine
                        {
                            Id = 1,
                            Translations = new List<SurveyCreateRequestTranslation>
                            {
                                new SurveyCreateRequestTranslation
                                {
                                    LanguageCode = "en",
                                    TranslationText = "Test Question"
                                }
                            }
                        },
                        QuestionOptions = new List<SurveyCreateRequestQuestionOption>
                        {
                            new SurveyCreateRequestQuestionOption
                            {
                                Id = 1,
                                OrderNumber = 1,
                                Translations = new List<SurveyCreateRequestTranslation>
                                {
                                    new SurveyCreateRequestTranslation
                                    {
                                        LanguageCode = "en",
                                        TranslationText = "Test Option"
                                    }
                                }
                            }
                        },
                        Type = 1
                    }
                }
            };
        }
    }
}
