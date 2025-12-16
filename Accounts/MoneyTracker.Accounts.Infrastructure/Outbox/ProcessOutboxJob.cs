using Microsoft.Extensions.Logging;
using MoneyTracker.Accounts.Application.Outbox;
using Quartz;

namespace MoneyTracker.Accounts.Infrastructure.Outbox;

[DisallowConcurrentExecution]
internal class ProcessOutboxJob : IJob
{
    private readonly IOutboxProcessor _outboxProcessor;
    private readonly ILogger<ProcessOutboxJob> _logger;

    public ProcessOutboxJob(IOutboxProcessor outboxProcessor, ILogger<ProcessOutboxJob> logger)
    {
        _outboxProcessor = outboxProcessor;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            _logger.LogDebug("Starting outbox processing...");
            int pendingMessagesCount = await _outboxProcessor.GetPendingMessagesCountAsync(context.CancellationToken);

            if (pendingMessagesCount > 0)
            {
                _logger.LogInformation("Processing {Count} pending outbox messages", pendingMessagesCount);
                await _outboxProcessor.ProcessPendingMessagesAsync(context.CancellationToken);
            }

            _logger.LogDebug("Outbox processing completed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occured while processing outbox messages");
            throw new JobExecutionException(ex, false);
        }
    }
}

