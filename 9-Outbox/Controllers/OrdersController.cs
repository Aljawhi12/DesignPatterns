using _9_Outbox.Data;
using _9_Outbox.Events;
using _9_Outbox.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace _9_Outbox.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _db;

        public OrdersController(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Create(string productName, decimal amount)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                ProductName = productName,
                Amount = amount,
                CreatedAt = DateTime.UtcNow
            };

            _db.Orders.Add(order);

            var orderCreatedEvent = new OrderCreatedEvent
            {
                OrderId = order.Id,
                ProductName = order.ProductName,
                Amount = order.Amount
            };

            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = nameof(OrderCreatedEvent),
                Payload = JsonSerializer.Serialize(orderCreatedEvent),
                CreatedAt = DateTime.UtcNow
            };

            _db.OutboxMessages.Add(outboxMessage);

            // Order + OutboxMessage are saved in one SaveChanges call,
            // which EF Core wraps in a single database transaction.
            await _db.SaveChangesAsync();

            return Ok(order);
        }
    }
}
