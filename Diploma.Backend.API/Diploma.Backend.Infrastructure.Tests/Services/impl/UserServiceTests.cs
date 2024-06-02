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
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private UserService _userService;

        [SetUp]
        public void SetUp()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepositoryMock.Object);
        }

        [Test]
        public async Task GetUserData_UserExists_ReturnsValidResponse()
        {
            // Arrange
            var userJwt = new User { Id = 1 };
            var subscription = new Subscription
            {
                UserId = 1,
                User = new User { Id = 1, LastName = "Test" }
            };

            _userRepositoryMock.Setup(repo => repo.GetSubscriptionByUserIdAsync(userJwt.Id))
                .ReturnsAsync(subscription);

            // Act
            var result = await _userService.GetUserData(userJwt);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual("Test", result.Data.LastName);
            Assert.IsNull(result.Error);
        }

        [Test]
        public async Task GetUserData_UserDoesNotExist_ReturnsErrorResponse()
        {
            // Arrange
            var userJwt = new User { Id = 1 };

            _userRepositoryMock.Setup(repo => repo.GetSubscriptionByUserIdAsync(userJwt.Id))
                .ReturnsAsync((Subscription)null);

            // Act
            var result = await _userService.GetUserData(userJwt);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.Data);
            Assert.IsNotNull(result.Error);
            Assert.AreEqual(ErrorCodes.SubscriptionOrUserNotFound.ToString(), result.Error.Message);
        }
    }
}
