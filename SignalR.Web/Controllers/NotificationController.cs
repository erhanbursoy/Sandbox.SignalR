using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationController(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage(string userId, string message)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
            {
                // Tüm kullanıcılara mesaj gönder
                await _hubContext.Clients.All.SendAsync("ReceiveNotification", message);
            }
            else
            {
                // Belirli bir kullanıcıya mesaj gönder
                await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", message);
            }

            return Ok("Mesaj başarıyla gönderildi.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Mesaj gönderimi sırasında hata: {ex.Message}");
        }
    }
}

public record NotificationRequest(string UserId, string Message);
