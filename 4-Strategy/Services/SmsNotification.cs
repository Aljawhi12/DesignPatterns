namespace _4_Strategy.Services
{
    public class SmsNotification : INotificationStrategy
    {
        public string Type => "sms";

        public void Send(string message)
        {
            Console.WriteLine($"Sending SMS: {message}");
        }
    }
}
