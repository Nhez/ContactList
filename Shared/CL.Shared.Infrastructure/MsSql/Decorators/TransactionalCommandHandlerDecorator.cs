using Humanizer;
using CL.Shared.Abstractions.Commands;
using CL.Shared.Abstractions.MsSql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CL.Shared.Infrastructure.MsSql.Decorators;

[Decorator]
public class TransactionalCommandHandlerDecorator<T> : ICommandHandler<T> where T : class, ICommand
{
    private readonly ICommandHandler<T> _handler;
    private readonly UnitOfWorkTypeRegistry _unitOfWorkTypeRegistry;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TransactionalCommandHandlerDecorator<T>> _logger;

    public TransactionalCommandHandlerDecorator(ICommandHandler<T> handler, UnitOfWorkTypeRegistry unitOfWorkTypeRegistry,
        IServiceProvider serviceProvider, ILogger<TransactionalCommandHandlerDecorator<T>> logger)
    {
        _handler = handler;
        _unitOfWorkTypeRegistry = unitOfWorkTypeRegistry;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task HandleAsync(T command, CancellationToken cancellationToken = default)
    {
        var unitOfWorkType = _unitOfWorkTypeRegistry.Resolve<T>();

        if (unitOfWorkType is null)
        {
            await _handler.HandleAsync(command, cancellationToken);
            return;
        }

        var unitOfWork = (IUnitOfWork) _serviceProvider.GetRequiredService(unitOfWorkType);
        var unitOfWorkName = unitOfWorkType.Name;
        var name = command.GetType().Name.Underscore();

        _logger.LogInformation("Handling: {Name} using TX ({UnitOfWorkName})...", name, unitOfWorkName);

        await unitOfWork.ExecuteAsync(() => _handler.HandleAsync(command, cancellationToken));

        _logger.LogInformation("Handled: {Name} using TX ({UnitOfWorkName})", name, unitOfWorkName);
    }
}

[Decorator]
public class TransactionalResultCommandHandlerDecorator<T, TResult> : IResultCommandHandler<T, TResult> where T : class, ICommand<TResult>
{
    private readonly IResultCommandHandler<T, TResult>  _handler;
    private readonly UnitOfWorkTypeRegistry _unitOfWorkTypeRegistry;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TransactionalCommandHandlerDecorator<T>> _logger;

    public TransactionalResultCommandHandlerDecorator(IResultCommandHandler<T, TResult>  handler, UnitOfWorkTypeRegistry unitOfWorkTypeRegistry,
        IServiceProvider serviceProvider, ILogger<TransactionalCommandHandlerDecorator<T>> logger)
    {
        _handler = handler;
        _unitOfWorkTypeRegistry = unitOfWorkTypeRegistry;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task<TResult> HandleAsync(T command, CancellationToken cancellationToken = default)
    {
        var unitOfWorkType = _unitOfWorkTypeRegistry.Resolve<T>();

        if (unitOfWorkType is null)
        {
            return await _handler.HandleAsync(command, cancellationToken);;
        }

        var unitOfWork = (IUnitOfWork) _serviceProvider.GetRequiredService(unitOfWorkType);
        var unitOfWorkName = unitOfWorkType.Name;
        var name = command.GetType().Name.Underscore();

        _logger.LogInformation("Handling: {Name} using TX ({UnitOfWorkName})...", name, unitOfWorkName);

        var data = await unitOfWork.ExecuteAsync<TResult>(() => _handler.HandleAsync(command, cancellationToken));

        _logger.LogInformation("Handled: {Name} using TX ({UnitOfWorkName})", name, unitOfWorkName);

        return data;
    }
}