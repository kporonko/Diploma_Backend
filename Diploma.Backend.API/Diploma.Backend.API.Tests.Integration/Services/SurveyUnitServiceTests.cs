using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Repositories;
using Diploma.Backend.Application.Services.impl;
using Diploma.Backend.Domain.Models;
using Diploma.Backend.Infrastructure.Repositories.impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Tests.Integration.Services
{
    [TestFixture]
    public class SurveyUnitServiceTests
    {
        private SurveyUnitService _surveyUnitService;
        private ISurveyUnitRepository _survUnitRepository;

        private readonly int SURVEY_UNIT_ID_TO_GET = 4;
        private readonly int USER_ID_TO_GET = 1;
        private readonly int EXPECTED_SURV_UNITS_COUNT = 1;

        [SetUp]
        public void Setup()
        {
            _survUnitRepository = new SurveyUnitRepository(new Infrastructure.Data.ApplicationContext());

            _surveyUnitService = new SurveyUnitService(_survUnitRepository);
        }

        [Test]
        public async Task GetSurveyUnitById_ValidId_ReturnsSurveyUnit()
        {
            // Arrange
            var surveyUnitId = SURVEY_UNIT_ID_TO_GET;
            var user = new User
            {
                Id = USER_ID_TO_GET
            };
            // Act
            var surveyUnit = await _surveyUnitService.GetSurveyUnit(user, surveyUnitId);

            // Assert
            Assert.IsNotNull(surveyUnit.Data);
            Assert.IsNull(surveyUnit.Error);
            Assert.AreEqual(surveyUnitId, surveyUnit.Data.Id);
        }

        [Test]
        public async Task GetSurveyUnitsUser_ValidId_ReturnsSurveyUnitsList()
        {
            // Arrange
            var user = new User
            {
                Id = USER_ID_TO_GET
            };
            // Act
            var surveyUnit = await _surveyUnitService.GetSurveyUnits(user);

            // Assert
            Assert.IsNotNull(surveyUnit.Data);
            Assert.IsNull(surveyUnit.Error);
            Assert.AreEqual(EXPECTED_SURV_UNITS_COUNT, surveyUnit.Data.Count);
        }


        [Test]
        public async Task CreateSurveyUnit_ValidData_ReturnsSurveyUnit()
        {
            // Arrange
            var user = new User
            {
                Id = USER_ID_TO_GET
            };
            var request = GetCreateRequest();
            // Act
            var surveyUnit = await _surveyUnitService.CreateSurveyUnit(user, request);

            // Assert
            Assert.IsNotNull(surveyUnit.Data);
            Assert.IsNull(surveyUnit.Error);
            Assert.AreEqual(request.Name, surveyUnit.Data.Name);
            Assert.AreEqual(request.AppearanceId, surveyUnit.Data.AppearanceIdName.Keys.First());
            Assert.AreEqual(request.HideAfterNoSurveys, surveyUnit.Data.HideAfterNoSurveys);
            Assert.AreEqual(request.MaximumSurveysPerDevice, surveyUnit.Data.MaximumSurveysPerDevice);
        }

        private SurveyUnitCreateRequest GetCreateRequest()
        {
            return new SurveyUnitCreateRequest
            {
                Name = "Test Survey Unit",
                AppearanceId = 1002,
                HideAfterNoSurveys = false,
                MaximumSurveysPerDevice = 1,
                MessageAfterNoSurveys = true,
                OneSurveyTakePerDevice = 1
            };
        }
    }
}
