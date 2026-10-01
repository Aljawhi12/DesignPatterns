namespace _4_Strategy.Services
{
    public class WhatsAppNotification : INotificationStrategy
    {
        public string Type => "whatsapp";

        public void Send(string message)
        {
            Console.WriteLine($"Sending WhatsApp: {message}");
        }
    }
}
