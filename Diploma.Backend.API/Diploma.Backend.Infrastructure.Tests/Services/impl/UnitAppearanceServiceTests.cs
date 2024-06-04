using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Repositories;
using Diploma.Backend.Application.Services.impl;
using Diploma.Backend.Domain.Enums;
using Diploma.Backend.Domain.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Tests.Services.impl
{
    [TestFixture]
    public class UnitAppearanceServiceTests
    {
        private Mock<IUnitAppearanceRepository> _unitAppearanceRepositoryMock;
        private UnitAppearanceService _unitAppearanceService;

        [SetUp]
        public void SetUp()
        {
            _unitAppearanceRepositoryMock = new Mock<IUnitAppearanceRepository>();
            _unitAppearanceService = new UnitAppearanceService(_unitAppearanceRepositoryMock.Object);
        }

        [Test]
        public async Task GetUnitAppearances_UserNotFound_ReturnsErrorResponse()
        {
            // Arrange
            _unitAppearanceRepositoryMock.Setup(repo => repo.GetUserWithUnitAppearancesAsync(It.IsAny<int>()))
                .ReturnsAsync((User)null);

            var userJwt = new User { Id = 1 };

            // Act
            var result = await _unitAppearanceService.GetUnitAppearances(userJwt);

            // Assert
            Assert.IsNotNull(result.Error);
            Assert.IsNull(result.Data);
            Assert.AreEqual(ErrorCodes.UserNotFound.ToString(), result.Error.Message);
        }

        [Test]
        public async Task GetUnitAppearances_ValidUser_ReturnsUnitAppearances()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                UnitAppearances = new List<UnitAppearance>
                {
                    new UnitAppearance { Id = 1, Name = "UA1", State = true, Template = new Template { Name = "Template1" }, Params = JsonSerializer.Serialize(new Dictionary<string, string>()) }
                }
            };

            _unitAppearanceRepositoryMock.Setup(repo => repo.GetUserWithUnitAppearancesAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            var userJwt = new User { Id = 1 };

            // Act
            var result = await _unitAppearanceService.GetUnitAppearances(userJwt);

            // Assert
            Assert.IsNotNull(result.Data);
            Assert.IsNull(result.Error);
            Assert.AreEqual(1, result.Data.Count);
        }

        [Test]
        public async Task CreateUnitAppearance_UserNotFound_ReturnsErrorResponse()
        {
            // Arrange
            _unitAppearanceRepositoryMock.Setup(repo => repo.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((User)null);

            var userJwt = new User { Id = 1 };
            var request = new UnitAppearanceCreateRequest { TemplateId = 1, Name = "Test", Type = "Default", Params = new Dictionary<string, string>() };

            // Act
            var result = await _unitAppearanceService.CreateUnitAppearance(userJwt, request);

            // Assert
            Assert.IsNotNull(result.Error);
            Assert.IsNull(result.Data);
            Assert.AreEqual(ErrorCodes.UserNotFound.ToString(), result.Error.Message);
        }

        [Test]
        public async Task CreateUnitAppearance_TemplateNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var user = new User { Id = 1 };

            _unitAppearanceRepositoryMock.Setup(repo => repo.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            _unitAppearanceRepositoryMock.Setup(repo => repo.GetTemplateByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Template)null);

            var userJwt = new User { Id = 1 };
            var request = new UnitAppearanceCreateRequest { TemplateId = 1, Name = "Test", Type = "Default", Params = new Dictionary<string, string>() };

            // Act
            var result = await _unitAppearanceService.CreateUnitAppearance(userJwt, request);

            // Assert
            Assert.IsNotNull(result.Error);
            Assert.IsNull(result.Data);
            Assert.AreEqual(ErrorCodes.TemplateNotFound.ToString(), result.Error.Message);
        }

        [Test]
        public async Task CreateUnitAppearance_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var user = new User { Id = 1 };
            var template = new Template { Id = 1, Name = "Template1" };

            _unitAppearanceRepositoryMock.Setup(repo => repo.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            _unitAppearanceRepositoryMock.Setup(repo => repo.GetTemplateByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(template);

            _unitAppearanceRepositoryMock.Setup(repo => repo.SaveUnitAppearanceAsync(It.IsAny<UnitAppearance>()))
                .Returns(Task.CompletedTask);

            var userJwt = new User { Id = 1 };
            var request = new UnitAppearanceCreateRequest { TemplateId = 1, Name = "Test", Type = "Right", Params = new Dictionary<string, string>() };

            // Act
            var result = await _unitAppearanceService.CreateUnitAppearance(userJwt, request);

            // Assert
            Assert.IsNotNull(result.Data);
            Assert.IsNull(result.Error);
            Assert.AreEqual("Test", result.Data.Name);
        }

        [Test]
        public async Task EditUnitAppearance_UserNotFound_ReturnsErrorResponse()
        {
            // Arrange
            _unitAppearanceRepositoryMock.Setup(repo => repo.GetUserWithUnitAppearancesAsync(It.IsAny<int>()))
                .ReturnsAsync((User)null);

            var userJwt = new User { Id = 1 };
            var request = new UnitAppearanceCreateRequest { Id = 1, TemplateId = 1, Name = "Test", Type = "Default", Params = new Dictionary<string, string>() };

            // Act
            var result = await _unitAppearanceService.EditUnitAppearance(userJwt, request);

            // Assert
            Assert.IsNotNull(result.Error);
            Assert.IsNull(result.Data);
            Assert.AreEqual(ErrorCodes.UserNotFound.ToString(), result.Error.Message);
        }

        [Test]
        public async Task EditUnitAppearance_UnitAppearanceNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var user = new User { Id = 1, UnitAppearances = new List<UnitAppearance>() };

            _unitAppearanceRepositoryMock.Setup(repo => repo.GetUserWithUnitAppearancesAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            var userJwt = new User { Id = 1 };
            var request = new UnitAppearanceCreateRequest { Id = 1, TemplateId = 1, Name = "Test", Type = "Default", Params = new Dictionary<string, string>() };

            // Act
            var result = await _unitAppearanceService.EditUnitAppearance(userJwt, request);

            // Assert
            Assert.IsNotNull(result.Error);
            Assert.IsNull(result.Data);
            Assert.AreEqual(ErrorCodes.UnitAppearanceNotFound.ToString(), result.Error.Message);
        }

        [Test]
        public async Task EditUnitAppearance_TemplateNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                UnitAppearances = new List<UnitAppearance>
                {
                    new UnitAppearance { Id = 1, Name = "UA1", State = true }
                }
            };

            _unitAppearanceRepositoryMock.Setup(repo => repo.GetUserWithUnitAppearancesAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            _unitAppearanceRepositoryMock.Setup(repo => repo.GetTemplateByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Template)null);

            var userJwt = new User { Id = 1 };
            var request = new UnitAppearanceCreateRequest { Id = 1, TemplateId = 1, Name = "Test", Type = "Default", Params = new Dictionary<string, string>() };

            // Act
            var result = await _unitAppearanceService.EditUnitAppearance(userJwt, request);

            // Assert
            Assert.IsNotNull(result.Error);
            Assert.IsNull(result.Data);
            Assert.AreEqual(ErrorCodes.TemplateNotFound.ToString(), result.Error.Message);
        }

        [Test]
        public async Task EditUnitAppearance_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                UnitAppearances = new List<UnitAppearance>
                {
                    new UnitAppearance { Id = 1, Name = "UA1", State = true }
                }
            };
            var template = new Template { Id = 1, Name = "Template1" };

            _unitAppearanceRepositoryMock.Setup(repo => repo.GetUserWithUnitAppearancesAsync(It.IsAny<int>()))
                .ReturnsAsync(user);

            _unitAppearanceRepositoryMock.Setup(repo => repo.GetTemplateByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(template);

            _unitAppearanceRepositoryMock.Setup(repo => repo.UpdateUnitAppearanceAsync(It.IsAny<UnitAppearance>()))
                .Returns(Task.CompletedTask);

            var userJwt = new User { Id = 1 };
            var request = new UnitAppearanceCreateRequest { Id = 1, TemplateId = 1, Name = "Test", Type = "Right", Params = new Dictionary<string, string>() };

            // Act
            var result = await _unitAppearanceService.EditUnitAppearance(userJwt, request);

            // Assert
            Assert.IsNotNull(result.Data);
            Assert.IsNull(result.Error);
            Assert.AreEqual("Test", result.Data.Name);
        }
    }
}
