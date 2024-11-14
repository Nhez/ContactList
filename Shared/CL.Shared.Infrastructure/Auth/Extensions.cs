using CL.Shared.Abstractions.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace CL.Shared.Infrastructure.Auth;

public static class Extensions
{
    public static IServiceCollection RegisterAuth(this IServiceCollection services)
    {
        services.AddTransient<ICurrentUserProvider, CurrentUserProvider>();

        return services;
    }
}