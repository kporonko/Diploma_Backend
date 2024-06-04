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
    public class UnitAppearanceServiceTests
    {
        private UnitAppearanceService _unitAppearanceService;
        private IUnitAppearanceRepository _unitAppearanceRepository;

        private readonly int USER_ID_TO_GET = 3;
        private readonly int EXPECTED_UA_COUNT = 8;

        [SetUp]
        public void Setup()
        {
            _unitAppearanceRepository = new UnitAppearanceRepository(new Infrastructure.Data.ApplicationContext());

            _unitAppearanceService = new UnitAppearanceService(_unitAppearanceRepository);
        }

        [Test]
        public async Task GetUnitAppearances_ValidUserId_ReturnsUnitAppearances()
        {
            // Arrange
            var user = new User
            {
                Id = USER_ID_TO_GET
            };
            // Act
            var response = await _unitAppearanceService.GetUnitAppearances(user);

            // Assert
            Assert.IsNotNull(response.Data);
            Assert.IsNull(response.Error);
            Assert.AreEqual(EXPECTED_UA_COUNT, response.Data.Count());
        }

        [Test]
        public async Task CreateUnitAppearance_ValidRequest_ReturnsUnitAppearance()
        {
            // Arrange
            var user = new User
            {
                Id = USER_ID_TO_GET
            };
            var request = GetCreateRequest();
            // Act
            var response = await _unitAppearanceService.CreateUnitAppearance(user, request);

            // Assert
            Assert.IsNotNull(response.Data);
            Assert.IsNull(response.Error);
            Assert.AreEqual(request.Name, response.Data.Name);
            Assert.AreEqual(request.Params, response.Data.Params);
            Assert.AreEqual(request.Type, response.Data.Type);
        }

        private UnitAppearanceCreateRequest GetCreateRequest()
        {
            return new UnitAppearanceCreateRequest
            {
                Name = "Test",
                Params = new Dictionary<string, string>
                {
                    { "param1", "value1" },
                    { "param2", "value2" }
                },
                TemplateId = 1,
                Type = "Right"
            };
        }
    }
}
