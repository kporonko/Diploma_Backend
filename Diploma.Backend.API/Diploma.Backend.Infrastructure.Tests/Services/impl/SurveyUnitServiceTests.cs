using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Repositories;
using Diploma.Backend.Application.Services;
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
    [TestFixture]
    public class SurveyUnitServiceTests
    {
        private Mock<ISurveyUnitRepository> _repositoryMock;
        private ISurveyUnitService _service;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<ISurveyUnitRepository>();
            _service = new SurveyUnitService(_repositoryMock.Object);
        }

        [Test]
        public async Task CreateSurveyUnit_UserNotFound_ReturnsUserNotFoundResponse()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync((User)null);

            // Act
            var result = await _service.CreateSurveyUnit(new User { Id = 1 }, new SurveyUnitCreateRequest());

            // Assert
            Assert.AreEqual(ErrorCodes.UserNotFound.ToString(), result.Error.Message);
        }

        [Test]
        public async Task CreateSurveyUnit_UnitAppearanceNotFound_ReturnsUnitAppearanceNotFoundResponse()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(new User { Id = 1 });
            _repositoryMock.Setup(r => r.GetUnitAppearanceByIdAsync(It.IsAny<int>())).ReturnsAsync((UnitAppearance)null);

            // Act
            var result = await _service.CreateSurveyUnit(new User { Id = 1 }, new SurveyUnitCreateRequest { AppearanceId = 1 });

            // Assert
            Assert.AreEqual(ErrorCodes.UnitAppearanceNotFound.ToString(), result.Error.Message);
        }

        [Test]
        public async Task CreateSurveyUnit_Success_ReturnsValidResponse()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(new User { Id = 1 });
            _repositoryMock.Setup(r => r.GetUnitAppearanceByIdAsync(It.IsAny<int>())).ReturnsAsync(new UnitAppearance { Id = 1 });
            _repositoryMock.Setup(r => r.AddSurveyUnitAsync(It.IsAny<SurveyUnit>())).Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateSurveyUnit(new User { Id = 1 }, GenerateCreateRequest());

            // Assert
            Assert.IsNull(result.Error);
            Assert.IsNotNull(result.Data);
        }

        private SurveyUnitCreateRequest GenerateCreateRequest()
        {
            return new SurveyUnitCreateRequest 
            {
                AppearanceId = 1,
                HideAfterNoSurveys = true,
                MaximumSurveysPerDevice = 1,
                MessageAfterNoSurveys = true,
                Name = "Name",
                OneSurveyTakePerDevice = 2
            };      
        }

        [Test]
        public async Task DeleteSurveyUnit_UserNotFound_ReturnsUserNotFoundResponse()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync((User)null);

            // Act
            var result = await _service.DeleteSurveyUnit(new User { Id = 1 }, new SurveyUnitDeleteRequest { Id = 1 });

            // Assert
            Assert.AreEqual(ErrorCodes.UserNotFound.ToString(), result.Error.Message);
        }

        [Test]
        public async Task DeleteSurveyUnit_SurveyUnitNotFound_ReturnsSurveyUnitNotFoundResponse()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(new User { Id = 1 });
            _repositoryMock.Setup(r => r.GetSurveyUnitByIdAsync(It.IsAny<int>())).ReturnsAsync((SurveyUnit)null);

            // Act
            var result = await _service.DeleteSurveyUnit(new User { Id = 1 }, new SurveyUnitDeleteRequest { Id = 1 });

            // Assert
            Assert.AreEqual(ErrorCodes.SurveyUnitNotFound.ToString(), result.Error.Message);
        }

        [Test]
        public async Task DeleteSurveyUnit_Success_ReturnsValidResponse()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(new User { Id = 1 });
            _repositoryMock.Setup(r => r.GetSurveyUnitByIdAsync(It.IsAny<int>())).ReturnsAsync(new SurveyUnit { Id = 1 });
            _repositoryMock.Setup(r => r.DeleteSurveyUnitAsync(It.IsAny<SurveyUnit>())).Returns(Task.CompletedTask);

            // Act
            var result = await _service.DeleteSurveyUnit(new User { Id = 1 }, new SurveyUnitDeleteRequest { Id = 1 });

            // Assert
            Assert.IsNull(result.Error);
            Assert.AreEqual("Survey unit deleted", result.Data);
        }

        [Test]
        public async Task EditSurveyUnit_UserNotFound_ReturnsUserNotFoundResponse()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync((User)null);

            // Act
            var result = await _service.EditSurveyUnit(new User { Id = 1 }, new SurveyUnitEditRequest());

            // Assert
            Assert.AreEqual(ErrorCodes.UserNotFound.ToString(), result.Error.Message);
        }

        [Test]
        public async Task EditSurveyUnit_UnitAppearanceNotFound_ReturnsUnitAppearanceNotFoundResponse()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(new User { Id = 1 });
            _repositoryMock.Setup(r => r.GetUnitAppearanceByIdAsync(It.IsAny<int>())).ReturnsAsync((UnitAppearance)null);

            // Act
            var result = await _service.EditSurveyUnit(new User { Id = 1 }, new SurveyUnitEditRequest { AppearanceId = 1 });

            // Assert
            Assert.AreEqual(ErrorCodes.UnitAppearanceNotFound.ToString(), result.Error.Message);
        }

        [Test]
        public async Task EditSurveyUnit_SurveyEmptySurveyUnitNotFound_ReturnsSurveyUnitNotFoundResponse()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(new User { Id = 1 });
            _repositoryMock.Setup(r => r.GetUnitAppearanceByIdAsync(It.IsAny<int>())).ReturnsAsync(new UnitAppearance { Id = 1 });
            _repositoryMock.Setup(r => r.GetSurveysByIdsAsync(It.IsAny<List<int>>())).ReturnsAsync(new List<Survey>());

            // Act
            var result = await _service.EditSurveyUnit(new User { Id = 1 }, new SurveyUnitEditRequest { AppearanceId = 1, SurveyIds = new List<int> { 1 } });

            // Assert
            Assert.AreEqual(ErrorCodes.SurveyUnitNotFound.ToString(), result.Error.Message);
        }

        [Test]
        public async Task EditSurveyUnit_Success_ReturnsValidResponse()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(new User { Id = 1 });
            _repositoryMock.Setup(r => r.GetUnitAppearanceByIdAsync(It.IsAny<int>())).ReturnsAsync(new UnitAppearance { Id = 1 });
            _repositoryMock.Setup(r => r.GetSurveysByIdsAsync(It.IsAny<List<int>>())).ReturnsAsync(new List<Survey> { new Survey { Id = 1 } });
            _repositoryMock.Setup(r => r.GetSurveyUnitByIdAsync(It.IsAny<int>())).ReturnsAsync(GetSurveyUnit());
            _repositoryMock.Setup(r => r.UpdateSurveyUnitAsync(It.IsAny<SurveyUnit>())).Returns(Task.CompletedTask);
            _repositoryMock.Setup(r => r.GetSurveysBySurveyUnitIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new List<Survey>()));
            _repositoryMock.Setup(r => r.DeleteSurveyInUnitsAsync(It.IsAny<List<SurveyInUnit>>())).Returns(Task.CompletedTask);
            _repositoryMock.Setup(r => r.AddSurveyInUnitsAsync(It.IsAny<List<SurveyInUnit>>())).Returns(Task.CompletedTask);
            _repositoryMock.Setup(r => r.GetSurveysBySurveyUnitIdAsync(It.IsAny<int>())).Returns(Task.FromResult(new List<Survey>()));
            
            // Act
            var result = await _service.EditSurveyUnit(new User { Id = 1 }, new SurveyUnitEditRequest { AppearanceId = 1, SurveyIds = new List<int> { 1 } });

            // Assert
            Assert.IsNull(result.Error);
            Assert.IsNotNull(result.Data);
        }

        private SurveyUnit GetSurveyUnit()
        {
            return new SurveyUnit
            {
                AppearanceId = 1,
                Id = 1,
                SurveyInUnits = new List<SurveyInUnit>
                {
                    new SurveyInUnit
                    {
                        Id = 1,
                        SurveyId = 1,
                        SurveyUnitId = 1
                    }
                },
                Name = "Name",
                UnitSettings = new UnitSettings
                {
                    HideAfterNoSurveys = true,
                    MaximumSurveysPerDevice = 1,
                    MessageAfterNoSurveys = true,
                    OneSurveyTakePerDevice = 2,
                },
                UnitAppearance = new UnitAppearance
                {
                    Id = 1,
                    Name = "Name",
                    Params = "Params",
                    State = true,
                }
            };
        }

        [Test]
        public async Task GetSurveyUnits_UserNotFound_ReturnsUserNotFoundResponse()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync((User)null);

            // Act
            var result = await _service.GetSurveyUnits(new User { Id = 1 });

            // Assert
            Assert.AreEqual(ErrorCodes.UserNotFound.ToString(), result.Error.Message);
        }

        [Test]
        public async Task GetSurveyUnits_Success_ReturnsValidResponse()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(new User { Id = 1 });
            _repositoryMock.Setup(r => r.GetSurveyUnitsByUserIdAsync(It.IsAny<int>())).ReturnsAsync(new List<SurveyUnit> { GetSurveyUnit() });
            _repositoryMock.Setup(r => r.GetSurveysBySurveyUnitIdAsync(It.IsAny<int>())).ReturnsAsync(new List<Survey> { new Survey { Id = 1, Name = "aaa" } });

            // Act
            var result = await _service.GetSurveyUnits(new User { Id = 1 });

            // Assert
            Assert.IsNull(result.Error);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(1, result.Data.Count);
        }

        [Test]
        public async Task GetSurveyUnit_UserNotFound_ReturnsUserNotFoundResponse()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync((User)null);

            // Act
            var result = await _service.GetSurveyUnit(new User { Id = 1 }, 1);

            // Assert
            Assert.AreEqual(ErrorCodes.UserNotFound.ToString(), result.Error.Message);
        }

        [Test]
        public async Task GetSurveyUnit_SurveyUnitNotFound_ReturnsSurveyUnitNotFoundResponse()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(new User { Id = 1, SurveyUnits = new List<SurveyUnit>() });

            // Act
            var result = await _service.GetSurveyUnit(new User { Id = 1 }, 1);

            // Assert
            Assert.AreEqual(ErrorCodes.SurveyUnitNotFound.ToString(), result.Error.Message);
        }

        [Test]
        public async Task GetSurveyUnit_Success_ReturnsValidResponse()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetUserByIdAsync(It.IsAny<int>())).ReturnsAsync(new User { Id = 1, SurveyUnits = new List<SurveyUnit> { new SurveyUnit { Id = 1 } } });

            // Act
            var result = await _service.GetSurveyUnit(new User { Id = 1 }, 1);

            // Assert
            Assert.IsNull(result.Error);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(1, result.Data.Id);
        }
    }
}
