public interface ISignalRClient
{
    Task SendMessageAsync(string message);
    Task ReceiveMessageAsync(Action<string> onMessageReceived);
}