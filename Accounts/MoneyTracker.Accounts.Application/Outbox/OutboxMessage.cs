using System.Text.Json;

namespace MoneyTracker.Accounts.Application.Outbox;

/// <summary>
/// Outbox-запись
/// </summary>
internal class OutboxMessage
{
    public Guid Id { get; private set; }
    public string Type { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;
    public DateTime OccurredOn { get; private set; }
    public DateTime? ProcessedOn { get; private set; }
    public string? Error { get; private set; }
    public int RetryCount { get; private set; }

    protected OutboxMessage() { }

    public static OutboxMessage Create<TEvent>(TEvent @event) where TEvent : class
    {
        return new OutboxMessage()
        {
            Id = Guid.NewGuid(),
            Type = typeof(TEvent).Name,
            Content = JsonSerializer.Serialize(@event, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }),
            OccurredOn = DateTime.UtcNow,
            RetryCount = 0
        };
    }

    public void MarkAsProcessed()
    {
        ProcessedOn = DateTime.UtcNow;
        Error = null;
    }

    public void MarkAsFailed(string error, int maxRetryCount = 5)
    {
        RetryCount++;
        Error = error;

        if (RetryCount >= maxRetryCount)
        {
            ProcessedOn = DateTime.UtcNow;
        }
    }

    public TEvent DeserializeContent<TEvent>() where TEvent : class
    {
        // TODO: надо прокинуть информацию по ошибке
        return JsonSerializer.Deserialize<TEvent>(Content)
            ?? throw new InvalidOperationException("moneytracker.accounts.outbox.unable_to_deserialize_event");
    }
}
