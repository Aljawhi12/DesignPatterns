using _4_Strategy.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _4_Strategy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StrategyController : ControllerBase
    {
        private readonly NotificationService _notificationService;

        public StrategyController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        public IActionResult Send(NotificationRequest request)
        {
            if (!_notificationService.TrySend(request.Type, request.Message))
            {
                return BadRequest(
                    $"Unknown notification type '{request.Type}'. " +
                    $"Supported: {string.Join(", ", _notificationService.SupportedTypes)}");
            }

            return Ok("Notification sent successfully");
        }
    }
}
