using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

[ApiController]
[Route("api/[controller]")]
public class ConnectedUsersController : ControllerBase
{
    [HttpGet]
    public IActionResult GetConnectedUsers()
    {
        try
        {
            // NotificationHub üzerindeki bağlı kullanıcı listesini al
            var connectedUsers = NotificationHub.GetConnectedUsers();
            return Ok(connectedUsers);
        }
        catch (Exception ex)
        {
            return BadRequest($"Hata: {ex.Message}");
        }
    }

}
