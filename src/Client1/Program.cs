using System.Text.Json;
using IdentityModel.Client;

async Task<string?> GetAccessTokenWithClientCredentialsGrantAsync(string issuerHost)
{
    var client = new HttpClient();
    var discoverEndpointResponse = await client.GetDiscoveryDocumentAsync(issuerHost);

    if (discoverEndpointResponse.IsError)
        throw new Exception(discoverEndpointResponse.Error);

    var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest()
    {
        Address = discoverEndpointResponse.TokenEndpoint,
        ClientId = "client",
        ClientSecret = "secret",
        Scope = "api1"
    });

    if (tokenResponse.IsError)
        throw new Exception(tokenResponse.Error);

    System.Console.WriteLine($"Incoming Access Token: {tokenResponse.AccessToken}");

    return tokenResponse.AccessToken;
}


string? accessToken = await GetAccessTokenWithClientCredentialsGrantAsync("https://localhost:5001");

if (accessToken is null)
    throw new Exception("Incoming Access Token is null");

var client = new HttpClient();
client.SetBearerToken(accessToken);

var response = await client.GetAsync("http://localhost:5245/identity");
if (!response.IsSuccessStatusCode)
    System.Console.WriteLine(response.StatusCode);

else
{
     var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;
    Console.WriteLine(JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true }));
}

Console.Read();