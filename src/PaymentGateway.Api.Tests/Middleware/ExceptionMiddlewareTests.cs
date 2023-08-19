
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentGateway.Api.Middleware;

namespace PaymentGateway.Api.Tests.Middleware
{
    [TestFixture]
    public class ExceptionMiddlewareTests
    {
        private Mock<ILoggerFactory> _mockFactory;

        [SetUp]
        public void BeforeEach()
        {
            _mockFactory = new Mock<ILoggerFactory>();
        }

        [Test]
        public async Task InvokeAsync_ShouldInvokeNextItemInMiddlewarePipeline()
        {
            // Data
            var context = Mock.Of<HttpContext>();
            var delegateCallCount = 0;

            // Setup
            var requestDelegate = new RequestDelegate(context =>
            {
                delegateCallCount++;
                return Task.CompletedTask;
            });

            var middleware = new ExceptionMiddleware(requestDelegate, _mockFactory.Object);

            // Test
            await middleware.InvokeAsync(context);

            // Analysis
            Assert.That(delegateCallCount, Is.EqualTo(1));
        }

        [Test]
        public void InvokeAsync_ShouldHandleExceptions()
        {
            // Data
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = $"/api/payments";
            var error = new Exception();

            // Setup
            var requestDelegate = new RequestDelegate(context =>
            {
                return Task.FromException(error);
            });

            var middleware = new ExceptionMiddleware(requestDelegate, _mockFactory.Object);

            // Test & Analysis
            Assert.That(
                async () => await middleware.InvokeAsync(httpContext),
                Throws.Nothing
            );
        }
    }
}