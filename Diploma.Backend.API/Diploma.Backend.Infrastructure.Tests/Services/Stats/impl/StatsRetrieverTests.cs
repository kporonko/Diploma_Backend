using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Application.Repositories.Stats;
using Diploma.Backend.Application.Services.Stats.impl;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Tests.Services.Stats.impl
{
    [TestFixture]
    public class StatsRetrieverTests
    {
        private IConfiguration _configuration;
        private Mock<IStatsRepository> _statsRepositoryMock;
        private StatsRetriever _statsRetriever;

        [SetUp]
        public void Setup()
        {
            // Создаем заглушку IConfiguration
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "VerticaSettings:Host", "localhost" },
                    { "VerticaSettings:Port", "5432" },
                    { "VerticaSettings:Database", "test" },
                    { "VerticaSettings:User", "user" },
                    { "VerticaSettings:Password", "password" }
                })
                .Build();

            // Создаем макет IStatsRepository
            _statsRepositoryMock = new Mock<IStatsRepository>();

            // Создаем экземпляр StatsRetriever с использованием IConfiguration и IStatsRepository
            _statsRetriever = new StatsRetriever(_configuration, _statsRepositoryMock.Object);
        }

        [Test]
        public async Task GetStats_WithInvalidQuestionId_ReturnsErrorMessage()
        {
            // Arrange
            int questionId = -1;

            // Устанавливаем поведение для заглушки IStatsRepository
            _statsRepositoryMock.Setup(repo => repo.GetStatsForQuestion(It.IsAny<int>())).ThrowsAsync(new ArgumentException("Invalid questionId"));

            // Act
            var response = await _statsRetriever.GetStats(questionId);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual("Invalid questionId", response.Error.Message);
        }

        [Test]
        public async Task GetStats_WhenRepositoryThrowsException_ReturnsErrorMessage()
        {
            // Arrange
            int questionId = 1;

            // Устанавливаем поведение для заглушки IStatsRepository
            _statsRepositoryMock.Setup(repo => repo.GetStatsForQuestion(It.IsAny<int>())).ThrowsAsync(new Exception("Repository exception"));

            // Act
            var response = await _statsRetriever.GetStats(questionId);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual("Repository exception", response.Error.Message);
        }

        [Test]
        public async Task GetStats_WhenRepositoryReturnsNull_ReturnsNullStats()
        {
            // Arrange
            int questionId = 1;

            // Устанавливаем поведение для заглушки IStatsRepository
            _statsRepositoryMock.Setup(repo => repo.GetStatsForQuestion(It.IsAny<int>())).ReturnsAsync((StatsForQuestion)null);
            _statsRepositoryMock.Setup(repo => repo.GetStatsForOption(It.IsAny<int>())).ReturnsAsync((List<StatsForOption>)null);
            _statsRepositoryMock.Setup(repo => repo.GetStatsForGender(It.IsAny<int>())).ReturnsAsync((List<StatsForGender>)null);
            _statsRepositoryMock.Setup(repo => repo.GetStatsForGeo(It.IsAny<int>())).ReturnsAsync((List<StatsForGeo>)null);
            _statsRepositoryMock.Setup(repo => repo.GetStatsForLang(It.IsAny<int>())).ReturnsAsync((List<StatsForLang>)null);

            // Act
            var response = await _statsRetriever.GetStats(questionId);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsNull(response.Data.StatsForQuestion);
            Assert.IsNull(response.Data.StatsForOption);
            Assert.IsNull(response.Data.StatsForGender);
            Assert.IsNull(response.Data.StatsForGeo);
            Assert.IsNull(response.Data.StatsForLang);
        }
    }
}
