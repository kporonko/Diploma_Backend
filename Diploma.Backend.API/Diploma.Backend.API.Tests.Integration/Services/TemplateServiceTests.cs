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
    public class TemplateServiceTests
    {
        private TemplateService _templateService;
        private ITemplateRepository _templateRepository;

        private readonly int EXPECTED_TEMPLATES_COUNT = 1;

        [SetUp]
        public void Setup()
        {
            _templateRepository = new TemplateRepository(new Infrastructure.Data.ApplicationContext());

            _templateService = new TemplateService(_templateRepository);
        }


        [Test]
        public async Task GetTemplates_ValidUserId_ReturnsTemplates()
        {
            // Arrange
            // Act
            var response = await _templateService.GetTemplates();

            // Assert
            Assert.IsNotNull(response.Data);
            Assert.IsNull(response.Error);
            Assert.AreEqual(EXPECTED_TEMPLATES_COUNT, response.Data.Count());
        }

        [Test]
        public async Task CreateTemplate_ValidUserId_ReturnsTemplate()
        {
            // Arrange
            // Act
            var request = GetCreateRequest();
            var response = await _templateService.CreateTemplate(request);

            // Assert
            Assert.IsNotNull(response.Data);
            Assert.IsNull(response.Error);
            Assert.AreEqual(request.Name, response.Data.Name);
            Assert.AreEqual(request.DefaultParams, response.Data.DefaultParams);
            Assert.AreEqual(request.TemplateCode, response.Data.TemplateCode);
        }

        private TemplateCreateRequest GetCreateRequest()
        {
            return new TemplateCreateRequest
            {
                Name = "Test",
                DefaultParams = new Dictionary<string, string>
                {
                    { "key", "value" }
                },
                TemplateCode = "code"
            };
        }
    }
}
