using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Repositories;
using Diploma.Backend.Application.Services.impl;
using Diploma.Backend.Domain.Models;
using Diploma.Backend.Infrastructure.Repositories.impl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Tests.Integration.Services
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        private AuthenticationService _authenticationService;
        private IAuthRepository _authRepository;
        private IConfiguration _configuration;

        [SetUp]
        public void Setup()
        {
            _configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: false)
               .Build();

            _authRepository = new AuthRepository(new Infrastructure.Data.ApplicationContext());

            _authenticationService = new AuthenticationService(_configuration, _authRepository);
        }
        
        [Test]
        public async Task Login_ValidCredentials_ReturnsSuccessfulResponse()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = "user@example.com",
                Password = "Stringst1"
            };

            // Act
            var response = await _authenticationService.Login(loginRequest);

            // Assert
            Assert.IsNotNull(response.Data);
            Assert.IsNull(response.Error);
            Assert.IsNotNull(response.Data.AccessToken);
        }

        [Test]
        public async Task Login_InvalidCredentials_ReturnsErrorResponse()
        {
            // Arrange
            var loginRequest = new LoginRequest
            {
                Email = "nonexistent@example.com",
                Password = "invalidpassword"
            };

            // Act
            var response = await _authenticationService.Login(loginRequest);

            // Assert
            Assert.IsNull(response.Data);
            Assert.IsNotNull(response.Error);
        }
    }
}