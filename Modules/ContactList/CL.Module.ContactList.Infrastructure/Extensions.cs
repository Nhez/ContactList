using CL.Module.ContactList.Application.Repositories;
using CL.Module.ContactList.Application.Services;
using CL.Module.ContactList.Application.Storages;
using CL.Module.ContactList.Infrastructure.EF;
using CL.Module.ContactList.Infrastructure.EF.Repositories;
using CL.Module.ContactList.Infrastructure.EF.Storages;
using CL.Module.ContactList.Infrastructure.Services;
using CL.Shared.Infrastructure.MsSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CL.Module.ContactList.Api")]
[assembly: InternalsVisibleTo("CL.Module.ContactList.Application")]
namespace CL.Module.ContactList.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IContactListRepository, ContactListRepository>();
        services.AddScoped<IContactListStorage, ContactListStorage>();
        services.AddScoped<IContactListService, ContactListService>();
        services.AddMSSql<ContactListContext>(configuration);

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        app.MigrateContext<ContactListContext>();

        return app;
    }
}
