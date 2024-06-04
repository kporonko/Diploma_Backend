using Diploma.Backend.Application.Repositories.Stats;
using Diploma.Backend.Application.Services.Stats.impl;
using Diploma.Backend.Application.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diploma.Backend.Infrastructure.Stats.Repositories;

namespace Diploma.Backend.Application.Tests.Integration.Services.Stats
{
    [TestFixture]
    public class StatsRetrieverTests
    {
        private IStatsRetriever _statsRetriever;
        private IStatsRepository _statsRepository;

        private readonly int QUESTION_ID_TO_TEST = 55;

        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json")
                .Build();

            _statsRepository = new StatsRepository(configuration);
            _statsRetriever = new StatsRetriever(configuration, _statsRepository);
        }

        [Test]
        public async Task GetStats_ValidQuestionId_ReturnsStats()
        {
            // Act
            var response = await _statsRetriever.GetStats(QUESTION_ID_TO_TEST);

            // Assert
            Assert.IsNotNull(response.Data);
            Assert.IsNull(response.Error);

            Assert.IsNotNull(response.Data.StatsForQuestion);
            Assert.AreEqual(QUESTION_ID_TO_TEST, response.Data.StatsForQuestion.QuestionId);

            Assert.IsNotNull(response.Data.StatsForOption);
            Assert.IsNotEmpty(response.Data.StatsForOption);
            Assert.AreEqual(QUESTION_ID_TO_TEST, response.Data.StatsForOption[0].QuestionId);

            Assert.IsNotNull(response.Data.StatsForGender);
            Assert.IsNotEmpty(response.Data.StatsForGender);
            Assert.AreEqual(QUESTION_ID_TO_TEST, response.Data.StatsForGender[0].QuestionId);

            Assert.IsNotNull(response.Data.StatsForGeo);
            Assert.IsNotEmpty(response.Data.StatsForGeo);
            Assert.AreEqual(QUESTION_ID_TO_TEST, response.Data.StatsForGeo[0].QuestionId);

            Assert.IsNotNull(response.Data.StatsForLang);
            Assert.IsNotEmpty(response.Data.StatsForLang);
            Assert.AreEqual(QUESTION_ID_TO_TEST, response.Data.StatsForLang[0].QuestionId);
        }

        [Test]
        public async Task GetStats_InvalidQuestionId_ReturnsError()
        {
            // Arrange
            var invalidQuestionId = -1;

            // Act
            var response = await _statsRetriever.GetStats(invalidQuestionId);

            // Assert
            Assert.IsNotNull(response.Data);
            Assert.IsNull(response.Data.StatsForQuestion);
            Assert.IsEmpty(response.Data.StatsForGeo);
            Assert.IsEmpty(response.Data.StatsForGender);
            Assert.IsEmpty(response.Data.StatsForLang);
            Assert.IsEmpty(response.Data.StatsForOption);
        }
    }
}
