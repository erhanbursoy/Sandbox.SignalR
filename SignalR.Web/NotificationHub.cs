using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

public class NotificationHub : Hub
{
    // Kullanıcı bağlantılarını saklamak için
    private static readonly ConcurrentDictionary<string, List<string>> UserConnections = new();

    public override Task OnConnectedAsync()
    {
        string userId = Context.User?.Identity?.Name; // Kullanıcı ID'sini al
        if (!string.IsNullOrEmpty(userId))
        {
            lock (UserConnections)
            {
                if (!UserConnections.ContainsKey(userId))
                    UserConnections[userId] = new List<string>();

                UserConnections[userId].Add(Context.ConnectionId);
            }
        }

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        string userId = Context.User?.Identity?.Name;
        if (!string.IsNullOrEmpty(userId))
        {
            lock (UserConnections)
            {
                if (UserConnections.ContainsKey(userId))
                {
                    UserConnections[userId].Remove(Context.ConnectionId);
                    if (UserConnections[userId].Count == 0)
                        UserConnections.TryRemove(userId, out _);
                }
            }
        }

        return base.OnDisconnectedAsync(exception);
    }

    public static IReadOnlyDictionary<string, List<string>> GetConnectedUsers()
    {
        return UserConnections;
    }
}
