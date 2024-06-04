using Diploma.Backend.Application.Repositories;
using Diploma.Backend.Application.Services;
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
    public class ServiceTests
    {
        private UserService _userService;
        private IUserRepository _userRepository;
        
        private readonly int USER_ID_TO_GET = 3;
        private readonly string SUBSCRIPTION_NUMBER = "I-NNK8RE3115WT";

        [SetUp]
        public void Setup()
        {
            _userRepository = new UserRepository(new Infrastructure.Data.ApplicationContext());

            _userService = new UserService(_userRepository);
        }

        [Test]
        public async Task GetUser_ValidUserId_ReturnsUser()
        {
            // Arrange
            var user = new User
            {
                Id = USER_ID_TO_GET
            };
            // Act
            var response = await _userService.GetUserData(user);

            // Assert
            Assert.IsNotNull(response.Data);
            Assert.IsNull(response.Error);
            Assert.AreEqual(USER_ID_TO_GET, response.Data.Id);
            Assert.AreEqual(SUBSCRIPTION_NUMBER, response.Data.Subscription.SubscriptionId);
        }
    }
}