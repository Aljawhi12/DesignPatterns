namespace _4_Strategy.Services
{
    public class EmailNotification : INotificationStrategy
    {
        public string Type => "email";

        public void Send(string message)
        {
            Console.WriteLine($"Sending Email: {message}");
        }
    }
}
