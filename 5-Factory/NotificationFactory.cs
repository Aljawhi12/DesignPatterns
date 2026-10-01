using _5_Factory.Services;

namespace _5_Factory
{
    public class NotificationFactory
    {
        public INotification Create(string type)
        {
            return type?.ToLowerInvariant() switch
            {
                "email" => new EmailNotification(),
                "sms" => new SmsNotification(),
                _ => throw new ArgumentException($"Invalid notification type '{type}'", nameof(type))
            };
        }
    }
}
