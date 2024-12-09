using SignalR.Lib;
using System.Net.Http.Json;

Console.WriteLine("Enter username:");
var username = Console.ReadLine();

Console.WriteLine("Enter password:");
var password = Console.ReadLine();

// API'den token al
var token = await GetTokenAsync(username, password);

var clientManager = new SignalRClientManager("https://localhost:7181/notificationHub", () => Task.FromResult(token));

clientManager.On<string>("ReceiveMessage", message =>
{
    Console.WriteLine($"Message received: {message}");
});

await clientManager.StartAsync();
Console.WriteLine("Connected to SignalR Hub. Press Enter to exit...");
Console.ReadLine();
await clientManager.StopAsync();

static async Task<string> GetTokenAsync(string username, string password)
{
    var httpClient = new HttpClient();
    var response = await httpClient.PostAsJsonAsync("https://localhost:7181/api/authentication/login", new { username, password });
    response.EnsureSuccessStatusCode();

    var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
    return tokenResponse?.Token ?? string.Empty;
}

record TokenResponse(string Token);
