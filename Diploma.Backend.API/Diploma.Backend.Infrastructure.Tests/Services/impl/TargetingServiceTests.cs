using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Application.Repositories;
using Diploma.Backend.Application.Services.impl;
using Diploma.Backend.Domain.Enums;
using Diploma.Backend.Domain.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Tests.Services.impl
{
    public class TargetingServiceTests
    {
        private Mock<ITargetingRepository> _repositoryMock;
        private TargetingService _service;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<ITargetingRepository>();
            _service = new TargetingService(_repositoryMock.Object);
        }

        [Test]
        public async Task CreateTargeting_UserNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var userJwt = new User { Id = 1 };
            var request = new TargetingCreateRequest();
            _repositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync((User)null);

            // Act
            var result = await _service.CreateTargeting(userJwt, request);

            // Assert
            Assert.IsNotNull(result.Error);
            Assert.IsNull(result.Data);
            Assert.AreEqual(ErrorCodes.UserNotFound.ToString(), result.Error.Message);
        }

        [Test]
        public async Task CreateTargeting_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var userJwt = new User { Id = 1 };
            var request = new TargetingCreateRequest
            {
                CountriesIds = new List<int> { 1, 2 }
            };
            var user = new User { Id = 1 };
            var country1 = new Country { Id = 1, Name = "Country1" };
            var country2 = new Country { Id = 2, Name = "Country2" };

            _repositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(user);
            _repositoryMock.Setup(r => r.GetCountriesByIdsAsync(It.IsAny<List<int>>())).ReturnsAsync(new List<Country> { country1, country2 });

            // Act
            var result = await _service.CreateTargeting(userJwt, request);

            // Assert
            Assert.IsNotNull(result.Data);
            Assert.IsNull(result.Error);
            Assert.AreEqual(request.CountriesIds.Count, result.Data.Countries.Count);
        }

        [Test]
        public async Task GetTargeting_UserNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var userJwt = new User { Id = 1 };
            int targetingId = 1;
            _repositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync((User)null);

            // Act
            var result = await _service.GetTargeting(userJwt, targetingId);

            // Assert
            Assert.IsNotNull(result.Error);
            Assert.IsNull(result.Data);
            Assert.AreEqual(ErrorCodes.UserNotFound.ToString(), result.Error.Message);
        }

        [Test]
        public async Task GetTargeting_TargetingNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var userJwt = new User { Id = 1 };
            int targetingId = 1;
            var user = new User { Id = 1 };
            _repositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(user);
            _repositoryMock.Setup(r => r.GetTargetingByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((Targeting)null);

            // Act
            var result = await _service.GetTargeting(userJwt, targetingId);

            // Assert
            Assert.IsNotNull(result.Error);
            Assert.IsNull(result.Data);
            Assert.AreEqual(ErrorCodes.TargetingNotFound.ToString(), result.Error.Message);
        }

        [Test]
        public async Task GetTargeting_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var userJwt = new User { Id = 1 };
            int targetingId = 1;
            var user = new User { Id = 1 };
            var targeting = new Targeting
            {
                Id = targetingId,
                UserId = user.Id,
                Name = "Targeting1",
                CountryInTargetings = new List<CountryInTargeting>
                {
                    new CountryInTargeting { Id = 1, Country = new Country { Id = 1, Name = "Country1" }, CountryId = 1 },
                    new CountryInTargeting { Id = 2, Country = new Country { Id = 2, Name = "Country2" }, CountryId = 2 }
                },
                Surveys = new List<Survey>
                {
                    new Survey { Id = 1, Name = "Survey1" },
                    new Survey { Id = 2, Name = "Survey2" }
                }
            };
            _repositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(user);
            _repositoryMock.Setup(r => r.GetTargetingByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(targeting);

            // Act
            var result = await _service.GetTargeting(userJwt, targetingId);

            // Assert
            Assert.IsNull(result.Error);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(targetingId, result.Data.Id);
        }

        [Test]
        public async Task EditTargeting_UserNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var userJwt = new User { Id = 1 };
            var request = new TargetingCreateRequest { Id = 1 };
            _repositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync((User)null);

            // Act
            var result = await _service.EditTargeting(userJwt, request);

            // Assert
            Assert.IsNotNull(result.Error);
            Assert.IsNull(result.Data);
            Assert.AreEqual(ErrorCodes.UserNotFound.ToString(), result.Error.Message);
        }

        [Test]
        public async Task EditTargeting_TargetingNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var userJwt = new User { Id = 1 };
            var request = new TargetingCreateRequest { Id = 1 };
            var user = new User { Id = 1 };
            _repositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(user);
            _repositoryMock.Setup(r => r.GetTargetingByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((Targeting)null);

            // Act
            var result = await _service.EditTargeting(userJwt, request);

            // Assert
            Assert.IsNotNull(result.Error);
            Assert.IsNull(result.Data);
            Assert.AreEqual(ErrorCodes.TargetingNotFound.ToString(), result.Error.Message);
        }

        [Test]
        public async Task EditTargeting_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var userJwt = new User { Id = 1 };
            var request = new TargetingCreateRequest
            {
                Id = 1,
                Name = "Updated Targeting",
                CountriesIds = new List<int> { 1, 2 }
            };
            var user = new User { Id = 1 };
            var targeting = new Targeting { Id = 1, UserId = user.Id, Name = "Original Targeting" };
            var country1 = new Country { Id = 1, Name = "Country1" };
            var country2 = new Country { Id = 2, Name = "Country2" };

            _repositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(user);
            _repositoryMock.Setup(r => r.GetTargetingByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(targeting);
            _repositoryMock.Setup(r => r.GetCountriesByIdsAsync(It.IsAny<List<int>>())).ReturnsAsync(new List<Country> { country1, country2 });

            // Act
            var result = await _service.EditTargeting(userJwt, request);

            // Assert
            Assert.IsNull(result.Error);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(request.Name, result.Data.Name);
        }

        [Test]
        public async Task GetTargetingsByUser_UserNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var userJwt = new User { Id = 1 };
            _repositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync((User)null);

            // Act
            var result = await _service.GetTargetingsByUser(userJwt);

            // Assert
            Assert.IsNotNull(result.Error);
            Assert.IsNull(result.Data);
            Assert.AreEqual(ErrorCodes.UserNotFound.ToString(), result.Error.Message);
        }

        [Test]
        public async Task GetTargetingsByUser_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var userJwt = new User { Id = 1 };
            var user = new User { Id = 1 };
            var targetings = new List<Targeting>
            {
                new Targeting { Id = 1, UserId = user.Id, Name = "Targeting1" },
                new Targeting { Id = 2, UserId = user.Id, Name = "Targeting2" }
            };

            _repositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(user);
            _repositoryMock.Setup(r => r.GetTargetingsByUserIdAsync(It.IsAny<int>())).ReturnsAsync(targetings);

            // Act
            var result = await _service.GetTargetingsByUser(userJwt);

            // Assert
            Assert.IsNotNull(result.Data);
            Assert.IsNull(result.Error);
            Assert.AreEqual(targetings.Count, result.Data.Count);
        }

        [Test]
        public async Task DeleteTargeting_UserNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var userJwt = new User { Id = 1 };
            int targetingId = 1;
            _repositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync((User)null);

            // Act
            var result = await _service.DeleteTargeting(userJwt, targetingId);

            // Assert
            Assert.IsNotNull(result.Error);
            Assert.IsNull(result.Data);
            Assert.AreEqual(ErrorCodes.UserNotFound.ToString(), result.Error.Message);
        }

        [Test]
        public async Task DeleteTargeting_TargetingNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var userJwt = new User { Id = 1 };
            int targetingId = 1;
            var user = new User { Id = 1 };
            _repositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(user);
            _repositoryMock.Setup(r => r.GetTargetingByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((Targeting)null);

            // Act
            var result = await _service.DeleteTargeting(userJwt, targetingId);

            // Assert
            Assert.IsNotNull(result.Error);
            Assert.IsNull(result.Data);
            Assert.AreEqual(ErrorCodes.TargetingNotFound.ToString(), result.Error.Message);
        }

        [Test]
        public async Task DeleteTargeting_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var userJwt = new User { Id = 1 };
            int targetingId = 1;
            var user = new User { Id = 1 };
            var targeting = new Targeting { Id = targetingId, UserId = user.Id };

            _repositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(user);
            _repositoryMock.Setup(r => r.GetTargetingByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(targeting);

            // Act
            var result = await _service.DeleteTargeting(userJwt, targetingId);

            // Assert
            Assert.IsNotNull(result.Data);
            Assert.IsNull(result.Error);
            Assert.AreEqual("Targeting deleted", result.Data);
        }

        [Test]
        public async Task GetAllCountries_ReturnsSuccessResponse()
        {
            // Arrange
            var countries = new List<CountryResponse>
            {
                new CountryResponse { Id = 1, Name = "Country1" },
                new CountryResponse { Id = 2, Name = "Country2" }
            };

            _repositoryMock.Setup(r => r.GetAllCountriesAsync()).ReturnsAsync(countries);

            // Act
            var result = await _service.GetAllCountries();

            // Assert
            Assert.IsNull(result.Error);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(countries.Count, result.Data.Count);
        }

        [Test]
        public async Task GetTargetingsCountries_UserNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var userJwt = new User { Id = 1 };
            int targetingId = 1;
            _repositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync((User)null);

            // Act
            var result = await _service.GetTargetingsCountries(userJwt, targetingId);

            // Assert
            Assert.IsNotNull(result.Error);
            Assert.IsNull(result.Data);
            Assert.AreEqual(ErrorCodes.UserNotFound.ToString(), result.Error.Message);
        }

        [Test]
        public async Task GetTargetingsCountries_TargetingNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var userJwt = new User { Id = 1 };
            int targetingId = 1;
            var user = new User { Id = 1 };
            _repositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(user);
            _repositoryMock.Setup(r => r.GetTargetingByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync((Targeting)null);

            // Act
            var result = await _service.GetTargetingsCountries(userJwt, targetingId);

            // Assert
            Assert.IsNotNull(result.Error);
            Assert.IsNull(result.Data);
            Assert.AreEqual(ErrorCodes.TargetingNotFound.ToString(), result.Error.Message);
        }

        [Test]
        public async Task GetTargetingsCountries_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var userJwt = new User { Id = 1 };
            int targetingId = 1;
            var user = new User { Id = 1 };
            var targeting = new Targeting
            {
                Id = targetingId,
                UserId = user.Id,
                CountryInTargetings = new List<CountryInTargeting>
                {
                    new CountryInTargeting { Country = new Country { Id = 1, Name = "Country1" } },
                    new CountryInTargeting { Country = new Country { Id = 2, Name = "Country2" } }
                }
            };
            _repositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(user);
            _repositoryMock.Setup(r => r.GetTargetingByIdAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(targeting);

            // Act
            var result = await _service.GetTargetingsCountries(userJwt, targetingId);

            // Assert
            Assert.IsNotNull(result.Data);
            Assert.IsNull(result.Error);
            Assert.AreEqual(targeting.CountryInTargetings.Count, result.Data.Count);
        }
    }
}