using FluentAssertions;
using IdentityModel.Client;
using Infrastructure.Transaction.Services.Models;
using Newtonsoft.Json;
using PaymentGateway.Api.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace PaymentGateway.Api.Integration.Tests
{
    // TODO: Determine instantiation failure on Gateway
    [TestFixture]
    public class PaymentsControllerTests
    {
        private TestWebApplicationFactory _factory;
        private TestIdWebApplicationFactory _factory2;
        private HttpClient _client;
        private HttpClient _client2;

        public PaymentsControllerTests()
        {
            _factory = new TestWebApplicationFactory();
            _client = _factory.CreateClient();

            _factory2 = new TestIdWebApplicationFactory();
            _client2 = _factory2.CreateClient();
        }

        [Test]
        public async Task Should_create_transaction_successfully()
        {
            // Arrange
            var payment = new CreateTransactionRequest(123, "John Smith", "0000222233334444", "02/23", "02/28", "123", 99.99M, "GBP");

            // Act
            var disco = await _client2.GetDiscoveryDocumentAsync(_client2.BaseAddress.AbsoluteUri);

            // request token
            var jwtResponse = await _client2.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "merchant",
                ClientSecret = "secret",
                Scope = "PaymentGateway.Api"
            });

            _client.SetBearerToken(jwtResponse.AccessToken);
            //_client.SetBearerToken("eyJhbGciOiJSUzI1NiIsImtpZCI6IkRBNzFCRDM1NTJGREJGMEFDQ0E4QkMyMERERDU0MTFCIiwidHlwIjoiYXQrand0In0.eyJuYmYiOjE2OTIzNTIzMjQsImV4cCI6MTY5MjM1NTkyNCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzIwNSIsImNsaWVudF9pZCI6Im1lcmNoYW50IiwianRpIjoiNERBMjA1OUM2MjI3N0QwNjFGOTBCMUJFNjNFNzE1MTAiLCJpYXQiOjE2OTIzNTIzMjQsInNjb3BlIjpbIlBheW1lbnRHYXRld2F5LkFwaSJdfQ.0RzTOelMZ4penlAKfLQ3JRNMLiTVOrQW6hffDOd-28jIejTKSQPL1lIcvhCwyVd5jEioWV9LxqLmbeGwwyVhKBpd4FMg2WdfFXlQXlwUUeWBwGWIWgyZbOuP5SEFaqw-FbeqgBsRiVagbS7PfK1UC7OM62IEFn7L4FFISqBp-8hEv6zvG62rl7AssHzrcUYVq-Wnl13LLRp7jCkqdRG5bmpCudsFbdiNh99VyEZ_T2HrGUKOY8-MYYULH_9By9lqoQQx6zMgNUyYOqeOv52ZkdXt0zZy6ZRhrCws5fKqZpo6ypUSWzMQVTJ7iXPgk6KnPdv_1kjgrFYkS9I7zwb1vg");

            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var stringContent = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(new Uri($"api/payments", UriKind.Relative), stringContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Content.Headers.ContentType!.MediaType.Should().Be("application/json");
            var responseModel = JsonConvert.DeserializeObject<CreateTransactionResponse>(
                response.Content.ReadAsStringAsync().Result);

            responseModel.Should().NotBeNull();
            responseModel.Should().BeOfType<CreateTransactionResponse>();
            responseModel.AcquirerTransactionId.Should().NotBeEmpty();
            responseModel.Status.Should().Be(TransactionStatus.Accepted);
            responseModel.TransactionId.Should().NotBeEmpty();
        }

        public async Task Should_create_transaction_unsuccessfully() { }
        public async Task Should_create_transaction_unauthorised() { }
        public async Task Should_get_transaction_successfully() { }
        public async Task Should_get_transaction_unsuccessfully() { }
    }
}