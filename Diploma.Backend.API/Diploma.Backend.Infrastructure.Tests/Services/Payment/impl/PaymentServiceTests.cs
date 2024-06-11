using Diploma.Backend.Application.Dto.Request;
using Diploma.Backend.Application.Dto.Response;
using Diploma.Backend.Application.Repositories.Payment.Proxies;
using Diploma.Backend.Application.Repositories.Payment;
using Diploma.Backend.Application.Services.Payment.impl;
using Diploma.Backend.Domain.Common;
using Diploma.Backend.Domain.Models;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using Diploma.Backend.Domain.Enums;

namespace Diploma.Backend.Application.Tests.Services.Payment.impl
{
    [TestFixture]
    public class PaymentServiceTests
    {
        private Mock<IPaymentProxy> _mockPaymentProxy;
        private Mock<IPaymentRepository> _mockPaymentRepository;
        private PaymentService _paymentService;

        [SetUp]
        public void Setup()
        {
            _mockPaymentProxy = new Mock<IPaymentProxy>();
            _mockPaymentRepository = new Mock<IPaymentRepository>();
            _paymentService = new PaymentService(_mockPaymentProxy.Object, _mockPaymentRepository.Object);
        }

        [Test]
        public async Task CreateSubscription_WhenNoExistingSubscription_ReturnsValidResponse()
        {
            // Arrange
            var request = new PayPalSubscriptionRequest();
            var user = new User();
            var tokenResponse = new PayPalAccessTokenResponse { access_token = "token" };
            var subscriptionResponse = new PayPalSubscriptionResponse();
            _mockPaymentProxy.Setup(p => p.GetTokenAsync()).ReturnsAsync(tokenResponse);
            _mockPaymentProxy.Setup(p => p.CreateSubscriptionAsync(request, "token")).ReturnsAsync(subscriptionResponse);
            _mockPaymentRepository.Setup(r => r.GetSubscriptionByUserId(It.IsAny<int>())).Returns((Subscription)null);
            _mockPaymentRepository.Setup(r => r.GetUserById(It.IsAny<int>())).Returns(user);

            // Act
            var result = await _paymentService.CreateSubscription(request, user);

            // Assert
            Assert.IsInstanceOf<BaseResponse<PayPalSubscriptionResponse>>(result);
            Assert.AreEqual(subscriptionResponse, result.Data);
        }

        [Test]
        public async Task CancelSubscription_WhenCalled_ReturnsValidResponse()
        {
            // Arrange
            var id = "subscriptionId";
            var request = new PayPalCancelSubscriptionRequest();
            var tokenResponse = new PayPalAccessTokenResponse { access_token = "token" };
            var cancelResponse = new PayPalCancelSubscriptionResponse();
            _mockPaymentProxy.Setup(p => p.GetTokenAsync()).ReturnsAsync(tokenResponse);
            _mockPaymentProxy.Setup(p => p.CancelSubscriptionAsync(id, request, "token")).ReturnsAsync(cancelResponse);

            // Act
            var result = await _paymentService.CancelSubscription(id, request);

            // Assert
            Assert.IsInstanceOf<BaseResponse<PayPalCancelSubscriptionResponse>>(result);
            Assert.AreEqual(cancelResponse, result.Data);
        }

        [Test]
        public async Task GetSubscription_WhenCalled_ReturnsValidResponse()
        {
            // Arrange
            var id = "subscriptionId";
            var tokenResponse = new PayPalAccessTokenResponse { access_token = "token" };
            var subscriptionResponse = new PayPalSubscriptionResponse();
            _mockPaymentProxy.Setup(p => p.GetTokenAsync()).ReturnsAsync(tokenResponse);
            _mockPaymentProxy.Setup(p => p.GetSubscriptionAsync(id, "token")).ReturnsAsync(subscriptionResponse);

            // Act
            var result = await _paymentService.GetSubscription(id);

            // Assert
            Assert.IsInstanceOf<BaseResponse<PayPalSubscriptionResponse>>(result);
            Assert.AreEqual(subscriptionResponse, result.Data);
        }

        [Test]
        public async Task CreateSubscription_WhenExistingSubscriptionIsActive_ReturnsError()
        {
            // Arrange
            var request = new PayPalSubscriptionRequest();
            var user = new User();
            var existingSubscription = new Subscription { Id = 1 };
            _mockPaymentProxy.Setup(p => p.GetTokenAsync()).ReturnsAsync(new PayPalAccessTokenResponse());
            _mockPaymentRepository.Setup(r => r.GetSubscriptionByUserId(It.IsAny<int>())).Returns(existingSubscription);

            // Act
            var result = await _paymentService.CreateSubscription(request, user);

            // Assert
            Assert.AreEqual(ErrorCodes.SubscriptionAlreadyExists.ToString(), result.Error.Message);
            Assert.IsNull(result.Data);
        }

        [Test]
        public async Task ActivateSubscription_WhenCalled_ReturnsValidResponseAndUpdateSubscription()
        {
            // Arrange
            var id = "subscriptionId";
            var request = new ActivateSubscriptionRequest();
            var tokenResponse = new PayPalAccessTokenResponse { access_token = "token" };
            var subscriptionResponse = new PayPalSubscriptionResponse { status_update_time = DateTime.Now };
            var subscription = new Subscription();
            _mockPaymentProxy.Setup(p => p.GetTokenAsync()).ReturnsAsync(tokenResponse);
            _mockPaymentProxy.Setup(p => p.ActivateSubscriptionAsync(id, request, "token")).ReturnsAsync(subscriptionResponse);
            _mockPaymentRepository.Setup(r => r.GetSubscriptionById(id)).Returns(subscription);

            // Act
            var result = await _paymentService.ActivateSubscription(id, request);

            // Assert
            _mockPaymentRepository.Verify(r => r.UpdateSubscription(subscription), Times.Once);
            Assert.IsInstanceOf<BaseResponse<PayPalSubscriptionResponse>>(result);
            Assert.AreEqual(subscriptionResponse, result.Data);
        }

        [Test]
        public async Task CreatePlan_WhenCalled_ReturnsValidResponse()
        {
            // Arrange
            var request = new PayPalPlanRequest();
            var tokenResponse = new PayPalAccessTokenResponse { access_token = "token" };
            var planResponse = new PayPalPlanResponse();
            _mockPaymentProxy.Setup(p => p.GetTokenAsync()).ReturnsAsync(tokenResponse);
            _mockPaymentProxy.Setup(p => p.CreatePlanAsync(request, "token")).ReturnsAsync(planResponse);

            // Act
            var result = await _paymentService.CreatePlan(request);

            // Assert
            Assert.IsInstanceOf<BaseResponse<PayPalPlanResponse>>(result);
            Assert.AreEqual(planResponse, result.Data);
        }

        [Test]
        public async Task CreateProduct_WhenCalled_ReturnsValidResponse()
        {
            // Arrange
            var request = new PayPalProductRequest();
            var tokenResponse = new PayPalAccessTokenResponse { access_token = "token" };
            var productResponse = new PayPalProductResponse();
            _mockPaymentProxy.Setup(p => p.GetTokenAsync()).ReturnsAsync(tokenResponse);
            _mockPaymentProxy.Setup(p => p.CreateProductAsync(request, "token")).ReturnsAsync(productResponse);

            // Act
            var result = await _paymentService.CreateProduct(request);

            // Assert
            Assert.IsInstanceOf<BaseResponse<PayPalProductResponse>>(result);
            Assert.AreEqual(productResponse, result.Data);
        }

        [Test]
        public async Task HandleExpiration_WhenSubscriptionExists_DeletesSubscription()
        {
            // Arrange
            var id = "subscriptionId";
            var subscription = new Subscription { SubscriptionId = id };
            _mockPaymentRepository.Setup(r => r.GetSubscriptionById(id)).Returns(subscription);

            // Act
            await _paymentService.HandleExpiration(id);

            // Assert
            _mockPaymentRepository.Verify(r => r.DeleteSubscriptionAsync(subscription), Times.Once);
        }

        [Test]
        public async Task HandleExpiration_WhenSubscriptionDoesNotExist_DoesNotDeleteSubscription()
        {
            // Arrange
            var id = "subscriptionId";
            _mockPaymentRepository.Setup(r => r.GetSubscriptionById(id)).Returns((Subscription)null);

            // Act
            await _paymentService.HandleExpiration(id);

            // Assert
            _mockPaymentRepository.Verify(r => r.DeleteSubscriptionAsync(It.IsAny<Subscription>()), Times.Never);
        }
    }
}
