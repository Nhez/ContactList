using CL.Shared.Abstractions.Commands;
using CL.Shared.Abstractions.Events;
using CL.Shared.Abstractions.Queries;

namespace CL.Shared.Abstractions.Dispatchers;

public interface IDispatcher
{
    Task SendAsync<T>(T command, CancellationToken cancellationToken = default) where T : class, ICommand;
    Task<TResult> SendAsync<T, TResult>(T command, CancellationToken cancellationToken = default) where T : class, ICommand<TResult>;
    Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class, IEvent;
    Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);
}
