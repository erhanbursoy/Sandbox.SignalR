using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Json;

Console.WriteLine("Username");
var username = Console.ReadLine();
Console.WriteLine("Password");
var password = Console.ReadLine();

// Kullanıcı adı ve şifre ile API'ye giriş yaparak token al
var httpClient = new HttpClient();
var loginResponse = await httpClient.PostAsJsonAsync("https://localhost:7181/api/authentication/login", new { Username = username, Password = password });

if (!loginResponse.IsSuccessStatusCode)
{
    Console.WriteLine("Login failed.");
    return;
}

var token = (await loginResponse.Content.ReadFromJsonAsync<TokenResponse>())?.Token;

if (string.IsNullOrEmpty(token))
{
    Console.WriteLine("Token is null or empty.");
    return;
}

// SignalR bağlantısını başlat
var connection = new HubConnectionBuilder()
    .WithUrl("https://localhost:7181/notificationHub", options =>
    {
        options.AccessTokenProvider = () => Task.FromResult(token);
    })
    .WithAutomaticReconnect()
    .Build();

connection.On<string>("ReceiveNotification", message =>
{
    Console.WriteLine($"Notification received: {message}");
});

try
{
    await connection.StartAsync();
    Console.WriteLine("Connected to the Notification Hub.");
}
catch (Exception ex)
{
    Console.WriteLine($"Failed to connect: {ex.Message}");
}

Console.WriteLine("Listening for notifications...");
Console.ReadLine();

public record TokenResponse(string Token);
