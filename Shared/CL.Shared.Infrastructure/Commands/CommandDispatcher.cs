using CL.Shared.Abstractions.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace CL.Shared.Infrastructure.Commands;

public sealed class CommandDispatcher : ICommandDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public CommandDispatcher(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;

    public async Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : class, ICommand
    {
        if (command is null)
        {
            return;
        }

        using var scope = _serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand>>();

        await handler.HandleAsync(command, cancellationToken);
    }

    public async Task<TResult> SendAsync<T, TResult>(T command, CancellationToken cancellationToken = default) where T : ICommand<TResult>
    {
        using var scope = _serviceProvider.CreateScope();
        var handler = scope.ServiceProvider.GetRequiredService<IResultCommandHandler<T, TResult>>();

        return await handler.HandleAsync(command, cancellationToken);
    }
}