namespace _4_Strategy.Services
{
    public interface INotificationStrategy
    {
        string Type { get; }

        void Send(string message);
    }
}
