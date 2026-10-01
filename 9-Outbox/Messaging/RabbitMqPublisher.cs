using System.Text;
using RabbitMQ.Client;

namespace _9_Outbox.Messaging;

// Registered as a singleton: one long-lived connection is reused for all publishes.
public sealed class RabbitMqPublisher : IAsyncDisposable
{
    private const string ExchangeName = "orders";
    private const string QueueName = "order-created";

    private readonly ConnectionFactory _factory;
    private readonly SemaphoreSlim _connectionLock = new(1, 1);
    private IConnection? _connection;

    public RabbitMqPublisher(IConfiguration configuration)
    {
        _factory = new ConnectionFactory
        {
            HostName = configuration["RabbitMQ:Host"] ?? "localhost",
            Port = int.TryParse(configuration["RabbitMQ:Port"], out var port) ? port : 5672,
            UserName = configuration["RabbitMQ:Username"] ?? "guest",
            Password = configuration["RabbitMQ:Password"] ?? "guest"
        };
    }

    public async Task PublishAsync(
        Guid messageId,
        string messageType,
        string payload,
        CancellationToken cancellationToken = default)
    {
        var connection = await GetConnectionAsync(cancellationToken);

        // Publisher confirms: BasicPublishAsync waits for the broker's ack
        // and throws if the message was nacked or could not be routed.
        await using var channel = await connection.CreateChannelAsync(
            new CreateChannelOptions(
                publisherConfirmationsEnabled: true,
                publisherConfirmationTrackingEnabled: true),
            cancellationToken);

        // 1. Create Exchange
        await channel.ExchangeDeclareAsync(
            exchange: ExchangeName,
            type: ExchangeType.Direct,
            durable: true,
            cancellationToken: cancellationToken);

        // 2. Create Queue
        await channel.QueueDeclareAsync(
            queue: QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: cancellationToken);

        // 3. Bind Queue to Exchange
        await channel.QueueBindAsync(
            queue: QueueName,
            exchange: ExchangeName,
            routingKey: messageType,
            cancellationToken: cancellationToken);

        // 4. Create message body + properties
        var body = Encoding.UTF8.GetBytes(payload);

        var properties = new BasicProperties
        {
            Persistent = true,                  // survive broker restart
            MessageId = messageId.ToString(),   // lets consumers de-duplicate
            Type = messageType,
            ContentType = "application/json"
        };

        // 5. Publish (mandatory: fail if no queue receives it)
        await channel.BasicPublishAsync(
            exchange: ExchangeName,
            routingKey: messageType,
            mandatory: true,
            basicProperties: properties,
            body: body,
            cancellationToken: cancellationToken);
    }

    private async Task<IConnection> GetConnectionAsync(CancellationToken cancellationToken)
    {
        if (_connection is { IsOpen: true })
            return _connection;

        await _connectionLock.WaitAsync(cancellationToken);
        try
        {
            if (_connection is { IsOpen: true })
                return _connection;

            if (_connection is not null)
                await _connection.DisposeAsync();

            _connection = await _factory.CreateConnectionAsync(cancellationToken);
            return _connection;
        }
        finally
        {
            _connectionLock.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection is not null)
            await _connection.DisposeAsync();

        _connectionLock.Dispose();
    }
}
