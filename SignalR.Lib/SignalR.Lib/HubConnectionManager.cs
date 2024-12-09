using Microsoft.AspNetCore.SignalR.Client;

public class HubConnectionManager
{
    public HubConnection Connection { get; private set; }

    public HubConnectionManager(string hubUrl)
    {
        Connection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .Build();
    }

    public async Task StartConnectionAsync()
    {
        await Connection.StartAsync();
    }

    public async Task StopConnectionAsync()
    {
        await Connection.StopAsync();
    }
}