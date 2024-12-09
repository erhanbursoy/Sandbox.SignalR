namespace SignalR.Lib
{
    using Microsoft.AspNetCore.SignalR.Client;
    using System;
    using System.Threading.Tasks;

    public class PrintHubClient
    {
        private readonly HubConnection _connection;

        public PrintHubClient(string hubUrl)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .Build();

            _connection.On<string>("ReceivePrintJob", async (document) =>
            {
                await PrintDocument(document);
            });
        }

        public async Task StartAsync()
        {
            await _connection.StartAsync();
        }

        public async Task StopAsync()
        {
            await _connection.StopAsync();
        }

        private Task PrintDocument(string document)
        {
            // Implement your printing logic here.
            Console.WriteLine("Printing document: " + document);
            return Task.CompletedTask;
        }
    }
}
