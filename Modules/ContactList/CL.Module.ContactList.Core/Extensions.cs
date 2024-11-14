using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CL.Module.ContactList.Api")]
[assembly: InternalsVisibleTo("CL.Module.ContactList.Infrastructure")]
[assembly: InternalsVisibleTo("CL.Module.ContactList.Application")]
namespace CL.Module.ContactList.Core;

public static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        return services;
    }
}
