using System.Reflection;
using CL.Shared.Abstractions.Messages.Contexts;
using CL.Shared.Infrastructure.Messages.Contexts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CL.Shared.Infrastructure.Messages;

public static class Extensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration, IEnumerable<Assembly> assemblies)
    {
        services.AddSingleton<IMessageContextProvider, MessageContextProvider>();
        services.AddSingleton<IMessageContextRegistry, MessageContextRegistry>();

        return services;
    }
}