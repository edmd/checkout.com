using Infrastructure.Transaction.Services.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using PaymentGateway.Api.Middleware;

namespace PaymentGateway.Api.Tests.Middleware
{
    //https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.logging.abstractions.nulllogger-1
    [TestFixture]
    public class ExceptionMiddlewareTests
    {
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

            var middleware = new ExceptionMiddleware(
                requestDelegate, NullLogger<ExceptionMiddleware>.Instance);

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

            var middleware = new ExceptionMiddleware(
                requestDelegate, NullLogger<ExceptionMiddleware>.Instance);

            // Test & Analysis
            Assert.That(
                async () => await middleware.InvokeAsync(httpContext),
                Throws.Nothing
            );
        }

        [Test]
        public void InvokeAsync_ShouldHandleTransactionNotFoundException()
        {
            // Data
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = $"/api/payments";
            var error = new TransactionNotFoundException("Exception");

            // Setup
            var requestDelegate = new RequestDelegate(context =>
            {
                return Task.FromException(error);
            });

            var middleware = new ExceptionMiddleware(
                requestDelegate, NullLogger<ExceptionMiddleware>.Instance);

            // Test & Analysis
            Assert.That(
                async () => await middleware.InvokeAsync(httpContext),
                Throws.Nothing
            );
        }

        [Test]
        public void InvokeAsync_ShouldHandleTransactionFailedException()
        {
            // Data
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = $"/api/payments";
            var error = new TransactionFailedException("Exception");

            // Setup
            var requestDelegate = new RequestDelegate(context =>
            {
                return Task.FromException(error);
            });

            var middleware = new ExceptionMiddleware(
                requestDelegate, NullLogger<ExceptionMiddleware>.Instance);

            // Test & Analysis
            Assert.That(
                async () => await middleware.InvokeAsync(httpContext),
                Throws.Nothing
            );
        }
    }
}