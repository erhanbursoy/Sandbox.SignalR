using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace SignalR.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationController(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost("sendToUser")]
        public async Task<IActionResult> SendToUser(string userId, string message)
        {
            await _hubContext.Clients.User(userId).SendAsync("ReceiveMessage", message);
            return Ok();
        }

        [HttpPost("broadcast")]
        public async Task<IActionResult> Broadcast(string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
            return Ok();
        }

        [HttpGet("connectedUsers")]
        public IActionResult GetConnectedUsers()
        {
            var users = NotificationHub.GetConnectedUsers();
            return Ok(users);
        }
    }
}
