using System.Collections.Concurrent;
using CL.Shared.Abstractions.Events;
using CL.Shared.Abstractions.Modules;
using CL.Shared.Abstractions.MsSql;
using Microsoft.EntityFrameworkCore;

namespace CL.Shared.Infrastructure.MsSql;

public abstract class MsSqlUnitOfWork<T> : IUnitOfWork where T : DbContext
{
    private readonly T _dbContext;
    private readonly Something _client;

    protected MsSqlUnitOfWork(
        T dbContext,
        Something client)
    {
        _dbContext = dbContext;
        _client = client;
    }

    public async Task ExecuteAsync(Func<Task> action)
    {
        await using var transaction = _dbContext.Database.CurrentTransaction ?? await _dbContext.Database.BeginTransactionAsync();

        try
        {
            await action();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> action)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            var data = await action();
            await transaction.CommitAsync();
            return data;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}

public class Something
{
    private readonly IModuleClient _client;
    public ConcurrentQueue<IEvent> Events { get; set; } = new ConcurrentQueue<IEvent>();

    public Something(IModuleClient client)
    {
        _client = client;
    }
    public void AddEvent(IEvent @event)
    {
        Events.Enqueue(@event);
    }

    public async Task PublishEventsAsync(CancellationToken cancellationToken = default)
    {
        while (Events.TryDequeue(out var @event))
        {
            await _client.PublishAsync(@event, cancellationToken);
        }
    }
}