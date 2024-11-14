using CL.Module.ContactList.Application;
using CL.Module.ContactList.Infrastructure;
using CL.Module.ContactList.Core;
using CL.Shared.Abstractions.Modules;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CL.Module.ContactList.Api
{
    [UsedImplicitly]
    internal sealed class ContactListModule : IModule
    {
        public string Name { get; } = "contact-list";

        public void Register(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddApplication()
                .AddCore()
                .AddInfrastructure(configuration);
        }

        public void Use(IApplicationBuilder app)
        {
            app.UseInfrastructure();
        }
    }
}
