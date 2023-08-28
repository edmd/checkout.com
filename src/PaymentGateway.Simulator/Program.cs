// See https://aka.ms/new-console-template for more information
using IdentityModel.Client;
using Newtonsoft.Json;
using PaymentGateway.Api.Models;
using System.Net.Http.Headers;
using System.Text;

TokenResponse? _jwtToken = null;
Guid? _transactionId = Guid.NewGuid();

Console.WriteLine("PaymentGateway Simulator\n");

try
{
    await FetchJwtToken();

    await SubmitPayment();

    await RetrievePaymentSuccess();

    await RetrievePaymentNotFound();

    await RetrievePaymentUnauthorised();

    await SubmitPaymentBadRequest();

    await SubmitPaymentGatewayDown();

    Console.ReadLine();
    return;
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}


async Task FetchJwtToken()
{
    Console.WriteLine("Fetch Jwt Token from Identity Server\n");

    // discover endpoints from metadata
    var client = new HttpClient();
    var disco = await client.GetDiscoveryDocumentAsync("https://localhost:7205");
    if (disco.IsError)
    {
        Console.WriteLine(disco.Error);
    }

    // request token
    var jwtResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
    {
        Address = disco.TokenEndpoint,
        ClientId = "merchant",
        ClientSecret = "secret",
        Scope = "PaymentGateway.Api"
    });

    if (jwtResponse.IsError)
    {
        Console.WriteLine(jwtResponse.Error);
    }

    Console.WriteLine($"JWT Token created: \n{jwtResponse.Json}\n");

    _jwtToken = jwtResponse;

    return;
}

async Task SubmitPayment()
{
    Console.WriteLine("Submit Payment for processing to the Gateway\n");

    // call api
    var client = new HttpClient();
    client.SetBearerToken(_jwtToken?.AccessToken);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    var payment = new CreateTransactionRequest(123, "John Smith", "0000222233334444", "02/23", "02/28", "123", 99.99M, "GBP");
    var stringContent = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json");
    var response = await client.PostAsync("https://localhost:7005/api/payments", stringContent);

    Console.WriteLine(response.StatusCode);

    if (response.IsSuccessStatusCode)
    {
        var content = await response.Content.ReadAsStringAsync();
        CreateTransactionResponse? transaction = JsonConvert.DeserializeObject<CreateTransactionResponse>(content);
        _transactionId = transaction?.TransactionId; // assign created transaction id
        Console.WriteLine(content);
        Console.WriteLine($"Transaction created with Id: {transaction?.TransactionId}\n");
    }

    return;
}

async Task RetrievePaymentSuccess()
{
    Console.WriteLine("Retrieve Payment information from Gateway (Success)\n");

    // call api
    var client = new HttpClient();
    client.SetBearerToken(_jwtToken?.AccessToken);

    var response = await client.GetAsync($"https://localhost:7005/api/payments/{_transactionId}");

    Console.WriteLine(response.StatusCode);

    if (response.IsSuccessStatusCode)
    {
        var content = await response.Content.ReadAsStringAsync();
        var transaction = JsonConvert.DeserializeObject<GetTransactionResponse>(content);
        Console.WriteLine(content);
        Console.WriteLine($"Transaction retrieved with Id: {transaction?.TransactionId}\n");
    }

    return;
}

async Task RetrievePaymentNotFound()
{
    Console.WriteLine("Retrieve Payment information from Gateway (Not Found)\n");

    // call api
    var client = new HttpClient();
    client.SetBearerToken(_jwtToken?.AccessToken);

    var response = await client.GetAsync($"https://localhost:7005/api/payments/{Guid.NewGuid()}");

    Console.WriteLine(response.StatusCode);

    if (response.IsSuccessStatusCode)
    {
        var content = await response.Content.ReadAsStringAsync();
        var transaction = JsonConvert.DeserializeObject<GetTransactionResponse>(content);
        Console.WriteLine(content);
        Console.WriteLine($"Transaction retrieved with Id: {transaction?.TransactionId}\n");
    } else {
        var content = await response.Content.ReadAsStringAsync();
        var transaction = JsonConvert.DeserializeObject<ErrorDetails>(content);
        Console.WriteLine($"{content}\n");
    }

    return;
}

async Task RetrievePaymentUnauthorised()
{
    Console.WriteLine("Retrieve Payment information from Gateway (Unauthorised)\n");

    // call api
    var client = new HttpClient();

    var response = await client.GetAsync($"https://localhost:7005/api/payments/{_transactionId}");

    Console.WriteLine($"{response.StatusCode}\n");

    return;
}

async Task SubmitPaymentBadRequest()
{
    Console.WriteLine("Submit Payment for processing to the Gateway (BadRequest)\n");

    // call api
    var client = new HttpClient();
    client.SetBearerToken(_jwtToken?.AccessToken);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    var payment = new CreateTransactionRequest(123, null, "0000222233334444", "02/23", "02/28", "123", 99.99M, "GBP");
    var stringContent = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json");
    var response = await client.PostAsync("https://localhost:7005/api/payments", stringContent);

    Console.WriteLine($"{response.StatusCode}\n");

    return;
}

async Task SubmitPaymentGatewayDown()
{
    Console.WriteLine("Submit Payment for processing to the Gateway (Gateway Down)\n");

    // call api
    var client = new HttpClient();
    client.SetBearerToken(_jwtToken?.AccessToken);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

    var payment = new CreateTransactionRequest(123, "John Smith", "1111222233334444", "02/23", "02/28", "123", 99.99M, "GBP");
    var stringContent = new StringContent(JsonConvert.SerializeObject(payment), Encoding.UTF8, "application/json");
    var response = await client.PostAsync("https://localhost:7005/api/payments", stringContent);

    Console.WriteLine($"{response.StatusCode}\n");

    return;
}