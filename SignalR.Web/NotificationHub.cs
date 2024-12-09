using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Security.Claims;

public class NotificationHub : Hub
{
    // Kullanıcıların bağlantılarını saklamak için bir ConcurrentDictionary
    private static readonly ConcurrentDictionary<string, List<string>> UserConnections = new();

    public override Task OnConnectedAsync()
    {
        string userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!string.IsNullOrEmpty(userId))
        {
            lock (UserConnections)
            {
                if (!UserConnections.ContainsKey(userId))
                {
                    UserConnections[userId] = new List<string>();
                }
                UserConnections[userId].Add(Context.ConnectionId);
            }
        }

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        string userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!string.IsNullOrEmpty(userId))
        {
            lock (UserConnections)
            {
                if (UserConnections.ContainsKey(userId))
                {
                    UserConnections[userId].Remove(Context.ConnectionId);
                    if (UserConnections[userId].Count == 0)
                    {
                        UserConnections.TryRemove(userId, out _);
                    }
                }
            }
        }

        return base.OnDisconnectedAsync(exception);
    }

    // Belirli bir kullanıcıya tüm bağlantıları üzerinden mesaj gönder
    public async Task SendMessageToUser(string userId, string message)
    {
        if (UserConnections.ContainsKey(userId))
        {
            foreach (var connectionId in UserConnections[userId])
            {
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
            }
        }
    }

    // Bağlı kullanıcıları dönen static bir metod
    public static Dictionary<string, List<string>> GetConnectedUsers()
    {
        return UserConnections.ToDictionary(k => k.Key, v => v.Value);
    }
}
