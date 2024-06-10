using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Application.Repositories;
using Diploma.Backend.Application.Services.impl;
using Diploma.Backend.Domain.Enums;
using Diploma.Backend.Domain.Models;
using Moq;
using NSubstitute;
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
            var userJwt = new User 
            {
                Id = 1,
                FirstName = "John",
                Subscription = new Subscription
                {
                    SubscriptionId = "sss",
                }
            };
            _userRepositoryMock.Setup(repo => repo.GetUserWithSubscription(userJwt.Id))
                .ReturnsAsync(userJwt);

            // Act
            var result = await _userService.GetUserData(userJwt);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Data);
            Assert.IsNotNull(result.Data.Subscription);
            Assert.AreEqual("John", result.Data.FirstName);
            Assert.AreEqual("sss", result.Data.Subscription.SubscriptionId);
            Assert.IsNull(result.Error);
        }

        [Test]
        public async Task GetUserData_UserDoesNotExist_ReturnsErrorResponse()
        {
            // Arrange
            var userJwt = new User { Id = 1 };

            _userRepositoryMock.Setup(repo => repo.GetUserWithSubscription(userJwt.Id))
                .ReturnsAsync(() => null);

            // Act
            var result = await _userService.GetUserData(userJwt);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.Data);
            Assert.IsNotNull(result.Error);
            Assert.AreEqual(ErrorCodes.UserNotFound.ToString(), result.Error.Message);
        }
    }
}
