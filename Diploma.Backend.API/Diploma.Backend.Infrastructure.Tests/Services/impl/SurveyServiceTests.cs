using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Application.Helpers;
using Diploma.Backend.Application.Mappers;
using Diploma.Backend.Application.Repositories;
using Diploma.Backend.Application.Services.impl;
using Diploma.Backend.Domain.Common;
using Diploma.Backend.Domain.Enums;
using Diploma.Backend.Domain.Models;
using Moq;
using NUnit.Framework;

namespace Diploma.Backend.Application.Tests.Services.impl
{
    [TestFixture]
    public class SurveyServiceTests
    {
        private Mock<ISurveyRepository> _surveyRepositoryMock;
        private SurveyService _surveyService;

        [SetUp]
        public void Setup()
        {
            _surveyRepositoryMock = new Mock<ISurveyRepository>();
            _surveyService = new SurveyService(_surveyRepositoryMock.Object);
        }

        [Test]
        public async Task CreateSurvey_ShouldReturnValidResponse_WhenUserExists()
        {
            // Arrange
            var userJwt = new User { Id = 1 };
            var surveyCreateRequest = FormAddRequest();
            var user = new User { Id = 1 };
            _surveyRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userJwt.Id)).ReturnsAsync(user);

            // Act
            var response = await _surveyService.CreateSurvey(userJwt, surveyCreateRequest);

            // Assert
            Assert.IsNotNull(response.Data);
            Assert.IsNull(response.Error);
            _surveyRepositoryMock.Verify(repo => repo.AddSurveyAsync(It.IsAny<Survey>()), Times.Once);
        }

        [Test]
        public async Task CreateSurvey_ShouldReturnErrorResponse_WhenUserDoesNotExist()
        {
            // Arrange
            var userJwt = new User { Id = 1 };
            var surveyCreateRequest = new SurveyCreateRequest { Name = "Test Survey" };
            _surveyRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userJwt.Id)).ReturnsAsync((User)null);

            // Act
            var response = await _surveyService.CreateSurvey(userJwt, surveyCreateRequest);

            // Assert
            Assert.IsNull(response.Data);
            Assert.IsNotNull(response.Error);
            Assert.AreEqual(ErrorCodes.UserNotFound.ToString(), response.Error.Message);
        }

        [Test]
        public async Task DeleteSurvey_ShouldReturnValidResponse_WhenUserAndSurveyExist()
        {
            // Arrange
            var userJwt = new User { Id = 1 };
            var surveyDeleteRequest = new SurveyDeleteRequest { Id = 1 };
            var user = new User { Id = 1 };
            _surveyRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userJwt.Id)).ReturnsAsync(user);

            // Act
            var response = await _surveyService.DeleteSurvey(userJwt, surveyDeleteRequest);

            // Assert
            Assert.IsNotNull(response.Data);
            Assert.IsNull(response.Error);
            _surveyRepositoryMock.Verify(repo => repo.DeleteSurveyAsync(surveyDeleteRequest.Id), Times.Once);
        }

        [Test]
        public async Task DeleteSurvey_ShouldReturnErrorResponse_WhenUserDoesNotExist()
        {
            // Arrange
            var userJwt = new User { Id = 1 };
            var surveyDeleteRequest = new SurveyDeleteRequest { Id = 1 };
            _surveyRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userJwt.Id)).ReturnsAsync((User)null);

            // Act
            var response = await _surveyService.DeleteSurvey(userJwt, surveyDeleteRequest);

            // Assert
            Assert.IsNull(response.Data);
            Assert.IsNotNull(response.Error);
            Assert.AreEqual(ErrorCodes.UserNotFound.ToString(), response.Error.Message);
        }

        [Test]
        public async Task EditSurvey_ShouldReturnValidResponse_WhenUserAndSurveyExist()
        {
            // Arrange
            var userJwt = new User { Id = 1 };
            var surveyEditRequest = new SurveyEditRequest { Id = 1, Name = "Edited Survey" };
            var user = new User { Id = 1 };
            var survey = new Survey { Id = 1 };
            _surveyRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userJwt.Id)).ReturnsAsync(user);
            _surveyRepositoryMock.Setup(repo => repo.GetSurveyByIdWithDetailsAsync(surveyEditRequest.Id)).ReturnsAsync(survey);

            // Act
            var response = await _surveyService.EditSurvey(userJwt, surveyEditRequest);

            // Assert
            Assert.IsNotNull(response.Data);
            Assert.IsNull(response.Error);
            _surveyRepositoryMock.Verify(repo => repo.UpdateSurveyAsync(It.IsAny<Survey>()), Times.Once);
        }

        [Test]
        public async Task EditSurvey_ShouldReturnValidResponseNoQuestions_WhenUserAndSurveyExist()
        {
            // Arrange
            var userJwt = new User { Id = 1 };
            var surveyEditRequest = FormEditRequest();
            var user = new User { Id = 1 };
            var survey = new Survey { Id = 1 };
            _surveyRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userJwt.Id)).ReturnsAsync(user);
            _surveyRepositoryMock.Setup(repo => repo.GetSurveyByIdWithDetailsAsync(surveyEditRequest.Id)).ReturnsAsync(survey);

            // Act
            var response = await _surveyService.EditSurvey(userJwt, surveyEditRequest);

            // Assert
            Assert.IsNotNull(response.Data);
            Assert.IsNull(response.Error);
            _surveyRepositoryMock.Verify(repo => repo.UpdateSurveyAsync(It.IsAny<Survey>()), Times.Once);
        }


        [Test]
        public async Task EditSurvey_ShouldReturnErrorResponse_WhenUserDoesNotExist()
        {
            // Arrange
            var userJwt = new User { Id = 1 };
            var surveyEditRequest = new SurveyEditRequest { Id = 1, Name = "Edited Survey" };
            _surveyRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userJwt.Id)).ReturnsAsync((User)null);

            // Act
            var response = await _surveyService.EditSurvey(userJwt, surveyEditRequest);

            // Assert
            Assert.IsNull(response.Data);
            Assert.IsNotNull(response.Error);
            Assert.AreEqual(ErrorCodes.UserNotFound.ToString(), response.Error.Message);
        }

        [Test]
        public async Task EditSurvey_ShouldReturnErrorResponse_WhenSurveyDoesNotExist()
        {
            // Arrange
            var userJwt = new User { Id = 1 };
            var surveyEditRequest = new SurveyEditRequest { Id = 1, Name = "Edited Survey" };
            var user = new User { Id = 1 };
            _surveyRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userJwt.Id)).ReturnsAsync(user);
            _surveyRepositoryMock.Setup(repo => repo.GetSurveyByIdWithDetailsAsync(surveyEditRequest.Id)).ReturnsAsync((Survey)null);

            // Act
            var response = await _surveyService.EditSurvey(userJwt, surveyEditRequest);

            // Assert
            Assert.IsNull(response.Data);
            Assert.IsNotNull(response.Error);
            Assert.AreEqual(ErrorCodes.SurveyNotFound.ToString(), response.Error.Message);
        }

        [Test]
        public async Task GetSurveyById_ShouldReturnValidResponse_WhenUserAndSurveyExist()
        {
            // Arrange
            var userJwt = new User { Id = 1 };
            var surveyId = 1;
            var user = new User { Id = 1 };
            var survey = new Survey { Id = 1 };
            _surveyRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userJwt.Id)).ReturnsAsync(user);
            _surveyRepositoryMock.Setup(repo => repo.GetSurveyByIdWithDetailsAsync(surveyId)).ReturnsAsync(survey);

            // Act
            var response = await _surveyService.GetSurveyById(userJwt, surveyId);

            // Assert
            Assert.IsNotNull(response.Data);
            Assert.IsNull(response.Error);
        }

        [Test]
        public async Task GetSurveyById_ShouldReturnErrorResponse_WhenUserDoesNotExist()
        {
            // Arrange
            var userJwt = new User { Id = 1 };
            var surveyId = 1;
            _surveyRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userJwt.Id)).ReturnsAsync((User)null);

            // Act
            var response = await _surveyService.GetSurveyById(userJwt, surveyId);

            // Assert
            Assert.IsNull(response.Data);
            Assert.IsNotNull(response.Error);
            Assert.AreEqual(ErrorCodes.UserNotFound.ToString(), response.Error.Message);
        }

        [Test]
        public async Task GetSurveyById_ShouldReturnErrorResponse_WhenSurveyDoesNotExist()
        {
            // Arrange
            var userJwt = new User { Id = 1 };
            var surveyId = 1;
            var user = new User { Id = 1 };
            _surveyRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userJwt.Id)).ReturnsAsync(user);
            _surveyRepositoryMock.Setup(repo => repo.GetSurveyByIdWithDetailsAsync(surveyId)).ReturnsAsync((Survey)null);

            // Act
            var response = await _surveyService.GetSurveyById(userJwt, surveyId);

            // Assert
            Assert.IsNull(response.Data);
            Assert.IsNotNull(response.Error);
            Assert.AreEqual(ErrorCodes.SurveyNotFound.ToString(), response.Error.Message);
        }

        [Test]
        public async Task GetUserSurveys_ShouldReturnValidResponse_WhenUserExists()
        {
            // Arrange
            var userJwt = new User { Id = 1 };
            var user = new User { Id = 1 };
            var surveys = new List<Survey> { new Survey { Id = 1 }, new Survey { Id = 2 } };
            _surveyRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userJwt.Id)).ReturnsAsync(user);
            _surveyRepositoryMock.Setup(repo => repo.GetUserSurveysAsync(user.Id)).ReturnsAsync(surveys);

            // Act
            var response = await _surveyService.GetUserSurveys(userJwt);

            // Assert
            Assert.IsNotNull(response.Data);
            Assert.IsNull(response.Error);
            Assert.AreEqual(2, response.Data.Count);
        }

        [Test]
        public async Task GetUserSurveys_ShouldReturnErrorResponse_WhenUserDoesNotExist()
        {
            // Arrange
            var userJwt = new User { Id = 1 };
            _surveyRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userJwt.Id)).ReturnsAsync((User)null);

            // Act
            var response = await _surveyService.GetUserSurveys(userJwt);

            // Assert
            Assert.IsNull(response.Data);
            Assert.IsNotNull(response.Error);
            Assert.AreEqual(ErrorCodes.UserNotFound.ToString(), response.Error.Message);
        }

        [Test]
        public async Task GetUserSurveys_ShouldReturnErrorResponse_WhenNoSurveys()
        {
            // Arrange
            var userJwt = new User { Id = 1 };
            var user = new User { Id = 1 };

            _surveyRepositoryMock.Setup(repo => repo.GetUserByIdAsync(userJwt.Id)).ReturnsAsync(user);
            _surveyRepositoryMock.Setup(repo => repo.GetUserSurveysAsync(user.Id)).ReturnsAsync((List<Survey>)null);

            // Act
            var response = await _surveyService.GetUserSurveys(userJwt);

            // Assert
            Assert.IsNull(response.Data);
            Assert.IsNotNull(response.Error);
            Assert.AreEqual(ErrorCodes.SurveyNotFound.ToString(), response.Error.Message);
        }

        private SurveyCreateRequest FormAddRequest()
        {
            return new SurveyCreateRequest
            {
                Name = "Test Survey",
                DateBy = DateTime.UtcNow,
                Questions = new List<SurveyCreateRequestQuestion>
                {
                    new SurveyCreateRequestQuestion
                    {
                        Id = 1,
                        Type = 1,
                        OrderNumber = 1,
                        QuestionLine = new SurveyCreateRequestQuestionLine
                        {
                            Id = 1,
                            Translations = new List<SurveyCreateRequestTranslation>
                            {
                                new SurveyCreateRequestTranslation { LanguageCode = "en", TranslationText = "Question 1" }
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
                                    new SurveyCreateRequestTranslation { LanguageCode = "en", TranslationText = "Option 1" }
                                }
                            }
                        }
                    }
                }
            };
        }

        private SurveyEditRequest FormEditRequest()
        {
            return new SurveyEditRequest
            {
                Id = 1,
                Name = "Edited Survey",
                DateBy = DateTime.UtcNow,
                Questions = new List<SurveyCreateRequestQuestion>
                {
                    new SurveyCreateRequestQuestion
                    {
                        Id = 1,
                        Type = 1,
                        OrderNumber = 1,
                        QuestionLine = new SurveyCreateRequestQuestionLine
                        {
                            Id = 1,
                            Translations = new List<SurveyCreateRequestTranslation>
                            {
                                new SurveyCreateRequestTranslation { LanguageCode = "en", TranslationText = "Edited Question 1" }
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
                                    new SurveyCreateRequestTranslation { LanguageCode = "en", TranslationText = "Edited Option 1" }
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
