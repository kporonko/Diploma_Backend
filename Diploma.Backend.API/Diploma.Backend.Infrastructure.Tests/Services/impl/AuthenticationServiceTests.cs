using NUnit.Framework;
using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Domain.Common;
using Diploma.Backend.Domain.Enums;
using Diploma.Backend.Domain.Models;
using Diploma.Backend.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using System.Threading.Tasks;
using Diploma.Backend.Domain.Extensions;
using Diploma.Backend.Application.Services.impl;
using Diploma.Backend.Application.Repositories;

namespace Diploma.Backend.Application.Tests.Services.impl
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        private IConfiguration _configuration;
        private IAuthRepository _authRepository;
        private AuthenticationService _authenticationService;

        [SetUp]
        public void Setup()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            _authRepository = Substitute.For<IAuthRepository>();
            _authenticationService = new AuthenticationService(_configuration, _authRepository);
        }

        [Test]
        public async Task Login_ValidCredentials_ReturnsValidResponse()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = "test@email.com",
                Password = "password",
            };

            var user = new User
            {
                Email = loginRequest.Email,
                Password = loginRequest.Password.ConvertPasswordToHash(),
                FirstName = "Test",
                LastName = "User",
                Role = Role.User
            };

            _authRepository.GetUserByEmailAsync(loginRequest.Email).Returns(Task.FromResult(user));

            // Act
            var result = await _authenticationService.Login(loginRequest);

            // Assert
            Assert.IsNotNull(result.Data);
            Assert.IsNotNull(result.Data.AccessToken);
        }

        [Test]
        public async Task Login_InvalidEmail_ReturnsErrorMessage()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = "nonexistent@email.com",
                Password = "password",
            };

            _authRepository.GetUserByEmailAsync(loginRequest.Email).Returns(Task.FromResult<User>(null));

            // Act
            var result = await _authenticationService.Login(loginRequest);

            // Assert
            Assert.IsNull(result.Data);
            Assert.IsNotNull(result.Error);
            Assert.AreEqual(ErrorCodes.UnexistingEmailException.ToString(), result.Error.Message);
        }

        [Test]
        public async Task Login_InvalidPassword_ReturnsErrorMessage()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = "test@email.com",
                Password = "$2a$10$1GOjLmgV8Biba1na0uMynu6DyHM./hhvy5YvUFhv1RLvKyI0JKWfO",
            };

            var user = new User
            {
                Email = loginRequest.Email,
                Password = "$2a$10$9zVSXQGdHXHG7FMMnIIGROUGunC1RbjRsfIpOKRQ9.TppFaNGPnEK",
            };

            _authRepository.GetUserByEmailAsync(loginRequest.Email).Returns(Task.FromResult(user));

            // Act
            var result = await _authenticationService.Login(loginRequest);

            // Assert
            Assert.IsNull(result.Data);
            Assert.IsNotNull(result.Error);
            Assert.AreEqual(ErrorCodes.InvalidPasswordException.ToString(), result.Error.Message);
        }

        [Test]
        public async Task Register_NewUser_ReturnsValidResponse()
        {
            // Arrange
            var registerRequest = new RegisterRequest
            {
                Email = "newuser@email.com",
                Password = "password",
                LastName = "NewUser",
                FirstName = "New",
            };

            _authRepository.GetUserByEmailAsync(registerRequest.Email).Returns(Task.FromResult<User>(null));

            // Act
            var result = await _authenticationService.Register(registerRequest);

            // Assert
            Assert.IsNull(result.Error);
            Assert.IsNotNull(result.Data);
            Assert.IsNotNull(result.Data.AccessToken);
        }

        [Test]
        public async Task Register_ExistingEmail_ReturnsErrorMessage()
        {
            // Arrange
            var registerRequest = new RegisterRequest
            {
                Email = "existing@email.com",
                Password = "password",
                LastName = "ExistingUser",
                FirstName = "Existing",
            };

            var existingUser = new User
            {
                Email = registerRequest.Email,
                Password = "existinghash",
            };

            _authRepository.GetUserByEmailAsync(registerRequest.Email).Returns(Task.FromResult(existingUser));

            // Act
            var result = await _authenticationService.Register(registerRequest);

            // Assert
            Assert.IsNull(result.Data);
            Assert.IsNotNull(result.Error);
            Assert.AreEqual(ErrorCodes.ExistingEmailException.ToString(), result.Error.Message);
        }
    }
}
