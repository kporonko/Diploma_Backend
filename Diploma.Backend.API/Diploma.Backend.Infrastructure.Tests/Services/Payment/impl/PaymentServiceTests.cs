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
        public async Task CancelSubscription_ValidId_ReturnsValidResponse()
        {
            // Arrange
            var id = "subscription_id";
            var cancelRequest = new PayPalCancelSubscriptionRequest();

            _mockPaymentProxy.Setup(p => p.GetTokenAsync()).ReturnsAsync(new PayPalAccessTokenResponse { access_token = "dummy_token" });
            _mockPaymentProxy.Setup(p => p.CancelSubscriptionAsync(id, cancelRequest, "dummy_token")).ReturnsAsync(new PayPalCancelSubscriptionResponse());

            // Act
            var result = await _paymentService.CancelSubscription(id, cancelRequest);

            // Assert
            Assert.IsNull(result.Error);
            Assert.IsNotNull(result.Data);
            Assert.IsInstanceOf<BaseResponse<PayPalCancelSubscriptionResponse>>(result);
        }

        [Test]
        public async Task CreateSubscription_NoExistingSubscription_ReturnsValidResponse()
        {
            // Arrange
            var request = new PayPalSubscriptionRequestShort();
            var jwtUser = new User { Id = 1 };

            _mockPaymentRepository.Setup(r => r.GetSubscriptionByUserId(jwtUser.Id)).Returns((Subscription)null);
            _mockPaymentProxy.Setup(p => p.GetTokenAsync()).ReturnsAsync(new PayPalAccessTokenResponse { access_token = "dummy_token" });
            _mockPaymentProxy.Setup(p => p.CreateProductAsync("dummy_token")).ReturnsAsync(new PayPalProductResponse { Id = "product_id" });
            _mockPaymentProxy.Setup(p => p.CreatePlanAsync("dummy_token", "product_id")).ReturnsAsync(new PayPalPlanResponse { Id = "plan_id" });
            _mockPaymentProxy.Setup(p => p.CreateSubscriptionAsync(request, "dummy_token", "plan_id")).ReturnsAsync(new PayPalSubscriptionResponse());

            _mockPaymentRepository.Setup(r => r.GetUserById(jwtUser.Id)).Returns(new User { Id = jwtUser.Id });
            _mockPaymentRepository.Setup(r => r.AddSubscriptionAsync(It.IsAny<Subscription>())).Returns(Task.CompletedTask);

            // Act
            var result = await _paymentService.CreateSubscription(request, jwtUser);

            // Assert
            Assert.IsNull(result.Error);
            Assert.IsNotNull(result.Data);
            Assert.IsInstanceOf<BaseResponse<PayPalSubscriptionResponse>>(result);
        }

        [Test]
        public async Task CreateSubscription_ExistingSubscription_ReturnsError()
        {
            // Arrange
            var request = new PayPalSubscriptionRequestShort();
            var jwtUser = new User { Id = 1 };
            var existingSubscription = new Subscription();

            _mockPaymentRepository.Setup(r => r.GetSubscriptionByUserId(jwtUser.Id)).Returns(existingSubscription);

            // Act
            var result = await _paymentService.CreateSubscription(request, jwtUser);

            // Assert
            Assert.IsNull(result.Data);
            Assert.IsNotNull(result.Error);
            Assert.AreEqual(ErrorCodes.SubscriptionAlreadyExists.ToString(), result.Error.Message);
        }

        [Test]
        public async Task GetSubscription_ValidId_ReturnsValidResponse()
        {
            // Arrange
            var id = "subscription_id";

            _mockPaymentProxy.Setup(p => p.GetTokenAsync()).ReturnsAsync(new PayPalAccessTokenResponse { access_token = "dummy_token" });
            _mockPaymentProxy.Setup(p => p.GetSubscriptionAsync(id, "dummy_token")).ReturnsAsync(new PayPalSubscriptionResponse());

            // Act
            var result = await _paymentService.GetSubscription(id);

            // Assert
            Assert.IsNull(result.Error);
            Assert.IsNotNull(result.Data);
            Assert.IsInstanceOf<BaseResponse<PayPalSubscriptionResponse>>(result);
        }

        [Test]
        public async Task ActivateSubscription_ValidId_ReturnsValidResponse()
        {
            // Arrange
            var id = "subscription_id";
            var activateRequest = new ActivateSubscriptionRequest();

            _mockPaymentProxy.Setup(p => p.GetTokenAsync()).ReturnsAsync(new PayPalAccessTokenResponse { access_token = "dummy_token" });
            _mockPaymentProxy.Setup(p => p.ActivateSubscriptionAsync(id, activateRequest, "dummy_token")).ReturnsAsync(new PayPalSubscriptionResponse());

            _mockPaymentRepository.Setup(r => r.GetSubscriptionById(id)).Returns(new Subscription { SubscriptionId = id });
            _mockPaymentRepository.Setup(r => r.UpdateSubscription(It.IsAny<Subscription>()));

            // Act
            var result = await _paymentService.ActivateSubscription(id, activateRequest);

            // Assert
            Assert.IsNull(result.Error);
            Assert.IsNotNull(result.Data);
            Assert.IsInstanceOf<BaseResponse<PayPalSubscriptionResponse>>(result);
        }
       

        [Test]
        public async Task HandleExpiration_ValidId_DeletesSubscription()
        {
            // Arrange
            var id = "subscription_id";
            _mockPaymentRepository.Setup(r => r.GetSubscriptionById(id)).Returns(new Subscription { SubscriptionId = id });
            _mockPaymentRepository.Setup(r => r.DeleteSubscriptionAsync(It.IsAny<Subscription>())).Returns(Task.CompletedTask);

            // Act
            await _paymentService.HandleExpiration(id);

            // Assert
            _mockPaymentRepository.Verify(r => r.DeleteSubscriptionAsync(It.IsAny<Subscription>()), Times.Once);
        }

        [Test]
        public async Task HandleExpiration_InvalidId_DoesNotDeleteSubscription()
        {
            // Arrange
            var id = "non_existing_subscription_id";
            _mockPaymentRepository.Setup(r => r.GetSubscriptionById(id)).Returns((Subscription)null);

            // Act
            await _paymentService.HandleExpiration(id);

            // Assert
            _mockPaymentRepository.Verify(r => r.DeleteSubscriptionAsync(It.IsAny<Subscription>()), Times.Never);
        }

    }
}
