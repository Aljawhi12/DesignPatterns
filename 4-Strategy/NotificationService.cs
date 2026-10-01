using _4_Strategy.Services;

namespace _4_Strategy
{
    // Strategy context: picks the right strategy at runtime and delegates to it.
    public class NotificationService
    {
        private readonly Dictionary<string, INotificationStrategy> _strategies;

        public NotificationService(IEnumerable<INotificationStrategy> strategies)
        {
            _strategies = strategies.ToDictionary(
                s => s.Type,
                StringComparer.OrdinalIgnoreCase);
        }

        public IEnumerable<string> SupportedTypes => _strategies.Keys;

        public bool TrySend(string type, string message)
        {
            if (!_strategies.TryGetValue(type, out var strategy))
                return false;

            strategy.Send(message);
            return true;
        }
    }
}
