using CL.Shared.Abstractions.Commands;
using CL.Shared.Abstractions.Dispatchers;
using CL.Shared.Abstractions.Events;
using CL.Shared.Abstractions.Queries;

namespace CL.Shared.Infrastructure.Dispatchers;

public class Dispatcher : IDispatcher
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IEventDispatcher _eventDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public Dispatcher(
        ICommandDispatcher commandDispatcher,
        IEventDispatcher eventDispatcher,
        IQueryDispatcher queryDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _eventDispatcher = eventDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    public Task SendAsync<T>(T command, CancellationToken cancellationToken = default) where T : class, ICommand
        => _commandDispatcher.SendAsync(command, cancellationToken);

    public Task<TResult> SendAsync<T, TResult>(T command, CancellationToken cancellationToken = default)
        where T : class, ICommand<TResult>
        => _commandDispatcher.SendAsync<T, TResult>(command, cancellationToken);

    public Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class, IEvent
    => _eventDispatcher.PublishAsync(@event, cancellationToken);

    public Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        => _queryDispatcher.QueryAsync(query, cancellationToken);
}