using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CL.Module.ContactList.Api")]
[assembly: InternalsVisibleTo("CL.Module.ContactList.Infrastructure")]
namespace CL.Module.ContactList.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}
