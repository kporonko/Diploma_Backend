using Diploma.Backend.Application.Helpers;
using Diploma.Backend.Domain.Enums;
using Diploma.Backend.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Diploma.Backend.Application.Tests.Helpers
{
    [TestFixture]
    public class CurrentUserRetrieverTests
    {
        [Test]
        public void GetCurrentUser_WithValidClaims_ReturnsValidBaseResponse()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, "test@example.com"),
                new Claim(ClaimTypes.GivenName, "John"),
                new Claim(ClaimTypes.Surname, "Doe"),
                new Claim(ClaimTypes.Role, Role.Admin.ToString()),
                new Claim(ClaimTypes.Sid, "123")
            };

            var identity = new ClaimsIdentity(claims);

            // Act
            var result = CurrentUserRetriever.GetCurrentUser(identity);

            // Assert
            Assert.IsNull(result.Error);
            Assert.IsNotNull(result.Data);
            Assert.IsInstanceOf<User>(result.Data);
            var user = result.Data;
            Assert.AreEqual("test@example.com", user.Email);
            Assert.AreEqual("John", user.FirstName);
            Assert.AreEqual("Doe", user.LastName);
            Assert.AreEqual(Role.Admin, user.Role);
            Assert.AreEqual(123, user.Id);
        }

        [Test]
        public void GetCurrentUser_WithMissingClaims_ReturnsErrorMessage()
        {
            // Arrange
            ClaimsIdentity identity = null;

            // Act
            var result = CurrentUserRetriever.GetCurrentUser(identity);

            // Assert
            Assert.IsNotNull(result.Error);
            Assert.IsNull(result.Data);
            Assert.AreEqual(ErrorCodes.UserNotFound.ToString(), result.Error.Message);
        }

        [Test]
        public void GetCurrentUser_WithInvalidClaims_ReturnsErrorMessage()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, "test@example.com"),
                new Claim(ClaimTypes.GivenName, "John"),
                new Claim(ClaimTypes.Surname, "Doe"),
                new Claim(ClaimTypes.Role, "InvalidRole"), // Invalid role
                new Claim(ClaimTypes.Sid, "123")
            };

            var identity = new ClaimsIdentity(claims);

            // Act
            var result = CurrentUserRetriever.GetCurrentUser(identity);

            // Assert
            Assert.IsNotNull(result.Error);
            Assert.IsNull(result.Data);
            Assert.AreEqual(ErrorCodes.InvalidUserClaims.ToString(), result.Error.Message);
        }

        [Test]
        public void GetCurrentUser_WithEmptyClaims_ReturnsErrorMessage()
        {
            // Arrange
            var claims = new List<Claim>();

            var identity = new ClaimsIdentity(claims);

            // Act
            var result = CurrentUserRetriever.GetCurrentUser(identity);

            // Assert
            Assert.IsNotNull(result.Error);
            Assert.IsNull(result.Data);
            Assert.AreEqual(ErrorCodes.InvalidUserClaims.ToString(), result.Error.Message);
        }

        [Test]
        public void GetCurrentUser_WithNullIdentity_ReturnsErrorMessage()
        {
            // Arrange
            ClaimsIdentity identity = null;

            // Act
            var result = CurrentUserRetriever.GetCurrentUser(identity);

            // Assert
            Assert.IsNotNull(result.Error);
            Assert.IsNull(result.Data);
            Assert.AreEqual(ErrorCodes.UserNotFound.ToString(), result.Error.Message);
        }
    }
}
