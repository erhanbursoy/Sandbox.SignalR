using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

public class SignalRClient : ISignalRClient
{
    private readonly HubConnection _connection;

    public SignalRClient(string hubUrl)
    {
        _connection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .Build();
    }

    public async Task SendMessageAsync(string message)
    {
        await _connection.InvokeAsync("SendMessage", message);
    }

    public async Task ReceiveMessageAsync(Action<string> onMessageReceived)
    {
        _connection.On("ReceiveMessage", onMessageReceived);
    }

    public async Task StartConnectionAsync()
    {
        await _connection.StartAsync();
    }

    public async Task StopConnectionAsync()
    {
        await _connection.StopAsync();
    }
}