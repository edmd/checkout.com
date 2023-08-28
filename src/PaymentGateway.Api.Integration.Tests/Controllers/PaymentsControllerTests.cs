using FluentAssertions;
using Infrastructure.Transaction.Services.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PaymentGateway.Api.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace PaymentGateway.Api.Integration.Tests
{
    [TestFixture]
    public class PaymentsControllerTests
    {
        private TestWebApplicationFactory _factory;
        private HttpClient _client;

        public PaymentsControllerTests()
        {
            _factory = new TestWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task Should_create_transaction_successfully()
        {
            // Arrange
            var payment = new CreateTransactionRequest(123, "John Smith", "0000222233334444", "02/23", "02/28", "123", 99.99M, "GBP");

            // Act
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                $"Bearer", $"{MockJwtTokens.GenerateJwtToken(MockJwtTokens.PaymentGatewayApiClaims)}");

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var stringContent = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(new Uri($"api/payments", UriKind.Relative), stringContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Content.Headers.ContentType!.MediaType.Should().Be("application/json");
            var responseModel = JsonConvert.DeserializeObject<CreateTransactionResponse>(
                response.Content.ReadAsStringAsync().Result);

            Assert.Multiple(() =>
            {
                responseModel.Should().NotBeNull();
                responseModel.Should().BeOfType<CreateTransactionResponse>();
                responseModel?.AcquirerTransactionId.Should().NotBeEmpty();
                responseModel?.Status.Should().Be(TransactionStatus.Accepted);
                responseModel?.TransactionId.Should().NotBeEmpty();
            });
        }

        [Test]
        public async Task Should_create_transaction_bad_request()
        {
            // Arrange
            var payment = new CreateTransactionRequest(123, null, "0000222233334444", "02/23", "02/28", "123", 99.99M, "GBP");

            // Act
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                $"Bearer", $"{MockJwtTokens.GenerateJwtToken(MockJwtTokens.PaymentGatewayApiClaims)}");

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var stringContent = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(new Uri($"api/payments", UriKind.Relative), stringContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Content.Headers.ContentType!.MediaType.Should().Be("application/problem+json");
            var responseContent = response.Content.ReadAsStringAsync().Result;

            Assert.Multiple(() =>
            {
                responseContent.Should().NotBeNull();
                responseContent.Should().Contain("The CardHolderName field is required");
                responseContent.Should().Contain("400");
            });
        }

        [Test]
        public async Task Should_create_transaction_gateway_down()
        {
            // Arrange
            var payment = new CreateTransactionRequest(123, "John Smith", "1111222233334444", "02/23", "02/28", "123", 99.99M, "GBP");

            // Act
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                $"Bearer", $"{MockJwtTokens.GenerateJwtToken(MockJwtTokens.PaymentGatewayApiClaims)}");

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var stringContent = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(new Uri($"api/payments", UriKind.Relative), stringContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            response.Content.Headers.ContentType!.MediaType.Should().Be("application/json");
            var responseModel = JsonConvert.DeserializeObject<ErrorDetails>(
                response.Content.ReadAsStringAsync().Result);

            Assert.Multiple(() =>
            {
                responseModel.Should().NotBeNull();
                responseModel.Should().BeOfType<ErrorDetails>();
                responseModel?.Message.Should().Be("Transaction rejected by Acquirer");
                responseModel?.StatusCode.Should().Be(500);
            });
        }

        [Test]
        public async Task Should_create_transaction_unauthorised()
        {
            // Arrange
            var payment = new CreateTransactionRequest(123, "John Smith", "0000222233334444", "02/23", "02/28", "123", 99.99M, "GBP");

            // Act
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                $"Bearer", $"");

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var stringContent = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(new Uri($"api/payments", UriKind.Relative), stringContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Test]
        public async Task Should_get_transaction_successfully() {
            // Arrange
            var payment = new CreateTransactionRequest(123, "John Smith", "0000222233334444", "02/23", "02/28", "123", 99.99M, "GBP");

            // Act
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                $"Bearer", $"{MockJwtTokens.GenerateJwtToken(MockJwtTokens.PaymentGatewayApiClaims)}");

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var stringContent = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(new Uri($"api/payments", UriKind.Relative), stringContent);
            var responseModel = JsonConvert.DeserializeObject<CreateTransactionResponse>(
                response.Content.ReadAsStringAsync().Result);

            var getResponse = await _client.GetAsync($"https://localhost:7005/api/payments/{responseModel?.TransactionId}");
            var content = await getResponse.Content.ReadAsStringAsync();
            var transaction = JsonConvert.DeserializeObject<GetTransactionResponse>(content);

            // Assert
            Assert.Multiple(() =>
            {
                transaction.Should().NotBeNull();
                transaction.Should().BeOfType<GetTransactionResponse>();
                transaction?.AcquirerTransactionId.Should().NotBeEmpty();
                transaction?.Amount.Should().Be(payment.Amount);
                transaction?.CardHolderName.Should().Be(payment.CardHolderName);
                transaction?.CardNumber.Should().Contain("0000");
                transaction?.CurrencyCode.Should().Be(payment.CurrencyCode);
                transaction?.Cvv2.Should().BeEmpty();
                transaction?.MerchantId.Should().Be(payment.MerchantId);
                transaction?.Status.Should().Be((int)TransactionStatus.Success);
                transaction?.TransactionId.Should().NotBeEmpty();
                transaction?.ValidFrom.Should().Be(payment.ValidFrom);
                transaction?.ValidTo.Should().Be(payment.ValidTo);
            });
        }

        [Test]
        public async Task Should_get_transaction_not_found() {
            // Act
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                $"Bearer", $"{MockJwtTokens.GenerateJwtToken(MockJwtTokens.PaymentGatewayApiClaims)}");

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var getResponse = await _client.GetAsync($"https://localhost:7005/api/payments/{Guid.NewGuid()}");
            var content = await getResponse.Content.ReadAsStringAsync();
            var errorDetails = JsonConvert.DeserializeObject<ErrorDetails>(content);

            // Assert
            Assert.Multiple(() =>
            {
                getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
                errorDetails?.Should().NotBeNull();
                errorDetails?.StatusCode.Should().Be(StatusCodes.Status404NotFound);
                errorDetails?.Message.Should().Be("Transaction not found in datastore");
            });
        }

        [Test]
        public async Task Should_get_transaction_unauthorised() {
            // Act
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                $"Bearer", $"");

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var getResponse = await _client.GetAsync($"https://localhost:7005/api/payments/{Guid.NewGuid()}");

            // Assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}