using System.Reflection;
using CL.Shared.Abstractions.Events;
using Microsoft.Extensions.DependencyInjection;

namespace CL.Shared.Infrastructure.Events;

public static class Extensions
{
    public static IServiceCollection AddEvents(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.AddSingleton<IEventDispatcher, EventDispatcher>();

        services.Scan(scanner => scanner.FromAssemblies(assemblies)
            .AddClasses(scannedClass => scannedClass.AssignableTo(typeof(IEventHandler<>))
                .WithoutAttribute<DecoratorAttribute>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}