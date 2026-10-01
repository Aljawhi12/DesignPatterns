using _8_Observer.Events;

namespace _8_Observer.Observers
{
    public class EmailObserver : IOrderObserver
    {
       
        public Task Handle(OrderCreatedEvent orderEvent)
        {
            Console.WriteLine(
                 $"Email sent for Order {orderEvent.OrderId}");

            return Task.CompletedTask;
        }
    }
}
