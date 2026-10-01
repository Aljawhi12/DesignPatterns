using _8_Observer.Events;
using _8_Observer.Observers;

namespace _8_Observer.Services
{
    public class OrderService
    {
        private readonly IEnumerable<IOrderObserver> _observers;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IEnumerable<IOrderObserver> observers,
            ILogger<OrderService> logger)
        {
            _observers = observers;
            _logger = logger;
        }

        public async Task CreateOrder(int orderId)
        {
            _logger.LogInformation("Creating Order {OrderId}...", orderId);

            var orderEvent = new OrderCreatedEvent(orderId);

            foreach (var observer in _observers)
            {
                // One failing observer must not stop the others from being notified.
                try
                {
                    await observer.Handle(orderEvent);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Observer {Observer} failed for Order {OrderId}",
                        observer.GetType().Name,
                        orderId);
                }
            }
        }
    }
}
