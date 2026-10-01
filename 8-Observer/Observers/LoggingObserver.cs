using _8_Observer.Events;

namespace _8_Observer.Observers
{
    public class LoggingObserver : IOrderObserver
    {

        public Task Handle(OrderCreatedEvent orderEvent)
        {
            Console.WriteLine(
            $"Order {orderEvent.OrderId} created");

            return Task.CompletedTask;
        }
    }
}
