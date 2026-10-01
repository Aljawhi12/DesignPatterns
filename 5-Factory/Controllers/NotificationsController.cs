using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _5_Factory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly NotificationFactory _factory;

        public NotificationsController(NotificationFactory factory)
        {
            _factory = factory;
        }

        [HttpPost]
        public IActionResult Send(string type, string message)
        {
            try
            {
                var notification = _factory.Create(type);

                notification.Send(message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}
