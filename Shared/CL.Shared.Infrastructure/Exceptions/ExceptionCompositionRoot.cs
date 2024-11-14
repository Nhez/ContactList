using CL.Shared.Abstractions.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace CL.Shared.Infrastructure.Exceptions;

public class ExceptionCompositionRoot : IExceptionCompositionRoot
{
    private readonly IServiceProvider _serviceProvider;

    public ExceptionCompositionRoot(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ExceptionResponse Map(Exception exception)
    {
        using var scope = _serviceProvider.CreateScope();
        var mappers = scope.ServiceProvider.GetServices<IExceptionResponseMapper>().ToArray();
        var nonDefaultMappers = mappers.Where(x => x is not ExceptionResponseMapper);
        var result = nonDefaultMappers
            .Select(x => x.Map(exception))
            .SingleOrDefault(x => x is not null);

        if (result is not null)
        {
            return result;
        }

        var defaultMapper = mappers.SingleOrDefault(x => x is ExceptionResponseMapper);

        return defaultMapper?.Map(exception);
    }
}