using _8_Observer.Events;

namespace _8_Observer.Observers
{
    public class SMSObserver : IOrderObserver
    {
       
        public Task Handle(OrderCreatedEvent orderEvent)
        {
            Console.WriteLine(
                 $"SMS sent for Order {orderEvent.OrderId}");

            return Task.CompletedTask;
        }
    }
}
