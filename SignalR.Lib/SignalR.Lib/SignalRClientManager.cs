using Microsoft.AspNetCore.SignalR.Client;

namespace SignalR.Lib
{
    public class SignalRClientManager
    {
        private readonly HubConnection _connection;

        public SignalRClientManager(string hubUrl, Func<Task<string>> accessTokenProvider = null)
        {
            var builder = new HubConnectionBuilder()
                .WithUrl(hubUrl, options =>
                {
                    if (accessTokenProvider != null)
                        options.AccessTokenProvider = accessTokenProvider;
                })
                .WithAutomaticReconnect();

            _connection = builder.Build();
        }

        public async Task StartAsync()
        {
            await _connection.StartAsync();
        }

        public async Task StopAsync()
        {
            await _connection.StopAsync();
        }

        public void On<T>(string methodName, Action<T> handler)
        {
            _connection.On(methodName, handler);
        }

        public async Task SendAsync(string methodName, params object[] args)
        {
            await _connection.SendAsync(methodName, args);
        }

        public HubConnectionState ConnectionState => _connection.State;

        public event Func<Exception?, Task> OnClosed
        {
            add => _connection.Closed += value;
            remove => _connection.Closed -= value;
        }

        public event Func<Exception?, Task> OnReconnecting
        {
            add => _connection.Reconnecting += value;
            remove => _connection.Reconnecting -= value;
        }

        public event Func<string?, Task> OnReconnected
        {
            add => _connection.Reconnected += value;
            remove => _connection.Reconnected -= value;
        }
    }
}
