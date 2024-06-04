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
    public class TargetingServiceTests
    {
        private TargetingService _targetingService;
        private ITargetingRepository _targetingRepository;

        private readonly int TARGETING_ID_TO_GET = 1;
        private readonly int USER_ID_TO_GET = 3;
        private readonly List<int> COUNTRY_IDS_TO_CREATE = new List<int> { 1, 2 };
        private readonly List<int> SURVEY_IDS_TO_CREATE = new List<int> { 1 };

        [SetUp]
        public void Setup()
        {
            _targetingRepository = new TargetingRepository(new Infrastructure.Data.ApplicationContext());

            _targetingService = new TargetingService(_targetingRepository);
        }

        [Test]
        public async Task GetTargetingByUserId_ValidUserId_ReturnsTargeting()
        {
            // Arrange
            var targetingId = TARGETING_ID_TO_GET;
            var user = new User
            {
                Id = USER_ID_TO_GET
            };
            // Act
            var response = await _targetingService.GetTargeting(user, targetingId);

            // Assert
            Assert.IsNotNull(response.Data);
            Assert.IsNull(response.Error);
        }
        
        [Test]
        public async Task CreateTargeting_ValidRequest_ReturnsTargeting()
        {
            // Arrange
            var user = new User
            {
                Id = USER_ID_TO_GET
            };
            var request = GteCreateRequest();
            // Act
            var response = await _targetingService.CreateTargeting(user, request);

            // Assert
            Assert.IsNotNull(response.Data);
            Assert.IsNull(response.Error);
            Assert.AreEqual(request.CountriesIds.Count, response.Data.Countries.Count);
        }

        private TargetingCreateRequest GteCreateRequest()
        {
            return new TargetingCreateRequest
            {
                Name = "Test Targeting",
                CountriesIds = COUNTRY_IDS_TO_CREATE,
                SurveyIds = SURVEY_IDS_TO_CREATE
            };
        }
    }
}