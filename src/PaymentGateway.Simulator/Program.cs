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

    await RetrievePayment();
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
        Console.WriteLine($"\nTransaction created with Id: {transaction?.TransactionId}\n");
    }

    return;
}

async Task RetrievePayment()
{
    Console.WriteLine("Retrieve Payment information from Gateway\n");

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
        Console.WriteLine($"\nTransaction retrieved with Id: {transaction?.TransactionId}");
    }

    Console.WriteLine($"\nPress any key to continue...");
    Console.ReadLine();
    return;
}