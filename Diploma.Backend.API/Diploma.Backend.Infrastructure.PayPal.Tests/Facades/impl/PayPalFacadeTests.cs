using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Infrastructure.PayPal.Facades.impl;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using RestSharp;
using System;

namespace Diploma.Backend.Infrastructure.PayPal.Tests.Facades.impl
{
    [TestFixture]
    public class PayPalFacadeTests
    {
        private PayPalFacade _payPalFacade;
        private Mock<IRestClient> _restClientMock;
        private IConfigurationRoot _configuration;

        [SetUp]
        public void SetUp()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _restClientMock = new Mock<IRestClient>();

            _payPalFacade = new PayPalFacade(_configuration, _restClientMock.Object);
        }

        [Test]
        public void Post_SuccessfulResponse_ReturnsDeserializedObject()
        {
            // Arrange
            var responseContent = "{\"accessToken\": {\"token\": \"testToken\"}}";
            var expectedToken = "testToken";
            var restResponse = new RestResponse { Content = responseContent, StatusCode = System.Net.HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed };
            _restClientMock.Setup(client => client.Execute(It.IsAny<IRestRequest>())).Returns(restResponse);

            // Act
            var result = _payPalFacade.Post<LoginResponse>("url");

            // Assert
            Assert.AreEqual(expectedToken, result.AccessToken.Token);
        }

        [Test]
        public void Post_UnsuccessfulResponse_ThrowsException()
        {
            // Arrange
            var restResponse = new RestResponse { StatusCode = System.Net.HttpStatusCode.BadRequest };
            _restClientMock.Setup(client => client.Execute(It.IsAny<IRestRequest>())).Returns(restResponse);

            // Act & Assert
            Assert.Throws<Exception>(() => _payPalFacade.Post<LoginResponse>("url"));
        }

        [Test]
        public void Get_SuccessfulResponse_ReturnsDeserializedObject()
        {
            // Arrange
            var responseContent = "{\"accessToken\": {\"token\": \"testToken\"}}";
            var expectedToken = "testToken";
            var restResponse = new RestResponse { Content = responseContent, StatusCode = System.Net.HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed };
            _restClientMock.Setup(client => client.Execute(It.IsAny<IRestRequest>())).Returns(restResponse);

            // Act
            var result = _payPalFacade.Get<LoginResponse>("url");

            // Assert
            Assert.AreEqual(expectedToken, result.AccessToken.Token);
        }

        [Test]
        public void Get_UnsuccessfulResponse_ThrowsException()
        {
            // Arrange
            var restResponse = new RestResponse { StatusCode = System.Net.HttpStatusCode.BadRequest };
            _restClientMock.Setup(client => client.Execute(It.IsAny<IRestRequest>())).Returns(restResponse);

            // Act & Assert
            Assert.Throws<Exception>(() => _payPalFacade.Get<LoginResponse>("url"));
        }

        [Test]
        public void Patch_SuccessfulResponse_ReturnsDeserializedObject()
        {
            // Arrange
            var responseContent = "{\"accessToken\": {\"token\": \"testToken\"}}";
            var expectedToken = "testToken";
            var restResponse = new RestResponse { Content = responseContent, StatusCode = System.Net.HttpStatusCode.OK, ResponseStatus = ResponseStatus.Completed };
            _restClientMock.Setup(client => client.Execute(It.IsAny<IRestRequest>())).Returns(restResponse);

            // Act
            var result = _payPalFacade.Patch<LoginResponse>("url");

            // Assert
            Assert.AreEqual(expectedToken, result.AccessToken.Token);
        }

        [Test]
        public void Patch_UnsuccessfulResponse_ThrowsException()
        {
            // Arrange
            var restResponse = new RestResponse { StatusCode = System.Net.HttpStatusCode.BadRequest };
            _restClientMock.Setup(client => client.Execute(It.IsAny<IRestRequest>())).Returns(restResponse);

            // Act & Assert
            Assert.Throws<Exception>(() => _payPalFacade.Patch<LoginResponse>("url"));
        }
    }
}
