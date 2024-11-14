namespace CL.Shared.Abstractions.Commands;

public interface ICommandDispatcher
{
    Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : class, ICommand;
    Task<TResult> SendAsync<T, TResult>(T command, CancellationToken cancellationToken = default) where T : ICommand<TResult>;
}
