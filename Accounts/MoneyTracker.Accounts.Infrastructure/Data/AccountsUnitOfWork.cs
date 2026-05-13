using MoneyTracker.Accounts.Application.Data;
using MoneyTracker.Accounts.Application.Outbox;
using MoneyTracker.Accounts.Domain.Accounts;
using MoneyTracker.Accounts.Domain.Categories;
using MoneyTracker.Accounts.Domain.Currencies;
using MoneyTracker.Accounts.Domain.Transfers;
using MoneyTracker.SharedKernel;
using MoneyTracker.SharedKernel.Interfaces;

namespace MoneyTracker.Accounts.Infrastructure.Data;

internal class AccountsUnitOfWork : IUnitOfWork
{
    private readonly AccountsDbContext _context;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public AccountsUnitOfWork(
        AccountsDbContext context,
        IDomainEventDispatcher domainEventDispatcher)
    {
        _context = context;
        _domainEventDispatcher = domainEventDispatcher;
    }

    public IAccountsRepository Accounts => throw new NotImplementedException();

    public ITransferRepository Transfers => throw new NotImplementedException();

    public ICurrencyRepository Currencies => throw new NotImplementedException();

    public IExpenseCategoryRepository ExpenseCategories => throw new NotImplementedException();

    public IIncomeCategoryRepository IncomeCategories => throw new NotImplementedException();

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        var publishedEvents = await PublishDomainEventsAsync(cancellationToken);
        await FillOutboxMessagesAsync(publishedEvents, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task<IEnumerable<DomainEvent>> PublishDomainEventsAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var changedDomainObjects = _context.ChangeTracker
            .Entries<IDomainObject>()
            ?.Select(e => e.Entity) ?? [];
        var domainEvents = changedDomainObjects
            .SelectMany(d => d.DomainEvents);
        await _domainEventDispatcher.DispatchAndClearEvents(changedDomainObjects);
        return domainEvents;
    }

    private async Task FillOutboxMessagesAsync(IEnumerable<DomainEvent> domainEvents, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        // TODO: преобразовать доменные события в интеграционные события
        var outboxMessages = domainEvents
            .Select(@event => OutboxMessage.Create(@event));
        await _context.Outbox.AddRangeAsync(outboxMessages, cancellationToken);
    }
}
