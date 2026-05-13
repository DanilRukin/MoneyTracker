using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoneyTracker.Accounts.Application.Eventing;
using MoneyTracker.Accounts.Application.Outbox;
using MoneyTracker.Accounts.Infrastructure.Data;
using Polly;
using Polly.Retry;
using MoneyTracker.SharedKernel;

namespace MoneyTracker.Accounts.Infrastructure.Outbox;

internal class OutboxProcessor : IOutboxProcessor
{
    private readonly IEventBus _eventBus;
    private readonly AccountsDbContext _context;
    private readonly ILogger<OutboxProcessor> _logger;
    private readonly AsyncRetryPolicy _policy;

    public OutboxProcessor(
        IEventBus eventBus,
        AccountsDbContext context,
        ILogger<OutboxProcessor> logger)
    {
        _eventBus = eventBus;
        _context = context;
        _logger = logger;
        _policy = CreatePolicy();
    }

    private AsyncRetryPolicy CreatePolicy()
    {
        return Policy.Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 5,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (exception, timeSpan, retryCount, context) =>
                {
                    _logger.LogWarning(
                            exception,
                            "Retry {RetryCount} after {Delay}ms for {PolicyKey}",
                            retryCount,
                            timeSpan.TotalMilliseconds,
                            context.PolicyKey);
                });
    }

    public async Task<int> GetPendingMessagesCountAsync(CancellationToken cancellationToken) =>
        await _context.Outbox.CountAsync(o => o.ProcessedOn == null && o.RetryCount< 5, cancellationToken);

    public async Task ProcessPendingMessagesAsync(CancellationToken cancellationToken)
    {
        List<OutboxMessage> pendingMessages = await _context
            .Outbox
            .Where(o => o.ProcessedOn == null && o.RetryCount < 5)
            .OrderBy(o => o.OccurredOn)
            .Take(100)
            .ToListAsync(cancellationToken) ?? new List<OutboxMessage>();

        foreach (var message in pendingMessages)
        {
            await ProcessMessageAsync(message, cancellationToken);
        }
    }

    private async Task ProcessMessageAsync(OutboxMessage message, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            return;
        try
        {
            await _policy.ExecuteAsync(async () =>
            {
                // TODO: использовать свойство Type объекта OutboxMessage для десериализации
                var domainEvent = message.DeserializeContent<DomainEvent>();
                await _eventBus.PublishAsync(domainEvent, cancellationToken);
            });            

            message.MarkAsProcessed();
            _logger.LogInformation("Outbox message {MessageId} processed successfully", message.Id);
            
        }
        catch (Exception ex)
        {
            message.MarkAsFailed(ex.Message);
            _logger.LogError(ex, "Failed to process outbox message {MessageId}", message.Id);
        }
        finally
        {
            await _context.SaveChangesAsync();
        }
    }
}
