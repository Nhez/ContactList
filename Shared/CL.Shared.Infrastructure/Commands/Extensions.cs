using System.Reflection;
using CL.Shared.Abstractions.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace CL.Shared.Infrastructure.Commands;

public static class Extensions
{
    public static IServiceCollection AddCommands(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.AddSingleton<ICommandDispatcher, CommandDispatcher>();

        services.Scan(scanner => scanner.FromAssemblies(assemblies)
            .AddClasses(scannedClass =>
                scannedClass.AssignableTo(typeof(ICommandHandler<>))
                    .WithoutAttribute<DecoratorAttribute>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Scan(scanner => scanner.FromAssemblies(assemblies)
            .AddClasses(scannedClass =>
                scannedClass.AssignableTo(typeof(IResultCommandHandler<,>))
                    .WithoutAttribute<DecoratorAttribute>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}