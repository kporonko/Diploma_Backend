using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Repositories;
using Diploma.Backend.Application.Services;
using Diploma.Backend.Application.Services.impl;
using Diploma.Backend.Domain.Enums;
using Diploma.Backend.Domain.Models;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Tests.Services.impl
{
    public class TemplateServiceTests
    {
        private Mock<ITemplateRepository> _repositoryMock;
        private ITemplateService _service;

        [SetUp]
        public void SetUp()
        {
            _repositoryMock = new Mock<ITemplateRepository>();
            _service = new TemplateService(_repositoryMock.Object);
        }

        [Test]
        public async Task CreateTemplate_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var request = new TemplateCreateRequest
            {
                Name = "Test Template",
                TemplateCode = "<html><body>This is a test template</body></html>",
                DefaultParams = new Dictionary<string, string>
                {
                    { "param1", "value1" },
                    { "param2", "value2" }
                }
            };

            var createdTemplate = new Template
            {
                Name = request.Name,
                TemplateCode = request.TemplateCode,
                DefaultParams = JsonConvert.SerializeObject(request.DefaultParams)
            };

            _repositoryMock.Setup(r => r.AddTemplateAsync(It.IsAny<Template>())).Returns(Task.FromResult(createdTemplate));

            // Act
            var result = await _service.CreateTemplate(request);

            // Assert
            Assert.IsNull(result.Error);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(createdTemplate.Name, result.Data.Name);
        }

        [Test]
        public async Task DeleteTemplate_TemplateNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var deleteRequest = new TemplateDeleteRequest { Id = 1 };
            _repositoryMock.Setup(r => r.GetTemplateByIdAsync(It.IsAny<int>())).ReturnsAsync((Template)null);

            // Act
            var result = await _service.DeleteTemplate(deleteRequest);

            // Assert
            Assert.IsNull(result.Data);
            Assert.IsNotNull(result.Error);
            Assert.AreEqual(ErrorCodes.TemplateNotFound.ToString(), result.Error.Message);
        }

        [Test]
        public async Task DeleteTemplate_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var deleteRequest = new TemplateDeleteRequest { Id = 1 };
            var templateToDelete = new Template { Id = deleteRequest.Id };
            _repositoryMock.Setup(r => r.GetTemplateByIdAsync(It.IsAny<int>())).ReturnsAsync(templateToDelete);

            // Act
            var result = await _service.DeleteTemplate(deleteRequest);

            // Assert
            Assert.IsNull(result.Error);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual("Template deleted", result.Data);
        }

        [Test]
        public async Task EditTemplate_TemplateNotFound_ReturnsErrorResponse()
        {
            // Arrange
            var editRequest = new TemplateEditRequest { Id = 1 };
            _repositoryMock.Setup(r => r.GetTemplateByIdAsync(It.IsAny<int>())).ReturnsAsync((Template)null);

            // Act
            var result = await _service.EditTemplate(editRequest);

            // Assert
            Assert.IsNull(result.Data);
            Assert.IsNotNull(result.Error);
            Assert.AreEqual(ErrorCodes.TemplateNotFound.ToString(), result.Error.Message);
        }

        [Test]
        public async Task EditTemplate_ValidRequest_ReturnsSuccessResponse()
        {
            // Arrange
            var editRequest = new TemplateEditRequest
            {
                Id = 1,
                Name = "Updated Template Name"
            };
            var templateToUpdate = new Template { Id = editRequest.Id };
            _repositoryMock.Setup(r => r.GetTemplateByIdAsync(It.IsAny<int>())).ReturnsAsync(templateToUpdate);

            // Act
            var result = await _service.EditTemplate(editRequest);

            // Assert
            Assert.IsNull(result.Error);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(editRequest.Name, result.Data.Name);
        }

        [Test]
        public async Task GetTemplates_ReturnsSuccessResponse()
        {
            // Arrange
            var templates = new List<Template>
            {
                new Template { Id = 1, Name = "Template 1" },
                new Template { Id = 2, Name = "Template 2" }
            };

            _repositoryMock.Setup(r => r.GetAllTemplatesAsync()).ReturnsAsync(templates);

            // Act
            var result = await _service.GetTemplates();

            // Assert
            Assert.IsNull(result.Error);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(templates.Count, result.Data.Count);
        }
    }
}
