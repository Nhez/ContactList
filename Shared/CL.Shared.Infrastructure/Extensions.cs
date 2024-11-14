using CL.Shared.Abstractions.Dispatchers;
using CL.Shared.Abstractions.Modules;
using CL.Shared.Infrastructure.Auth;
using CL.Shared.Infrastructure.Commands;
using CL.Shared.Infrastructure.Dispatchers;
using CL.Shared.Infrastructure.Events;
using CL.Shared.Infrastructure.Exceptions;
using CL.Shared.Infrastructure.Messages;
using CL.Shared.Infrastructure.Modules;
using CL.Shared.Infrastructure.Queries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace CL.Shared.Infrastructure
{
    public static class Extensions
    {
        private static IList<Assembly> _assemblies;
        private static IList<IModule> _modules;
        private static IConfiguration _configuration;

        private static readonly string Environment
        = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

        public static ConfigurationManager RegisterConfiguration(this ConfigurationManager configuration, Type programType)
        {
            configuration.SetBasePath(Directory.GetCurrentDirectory());
            configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            configuration.AddJsonFile($"appsettings.{Environment}.json", optional: true);
            configuration.AddUserSecrets(Assembly.GetAssembly(programType), optional: true);
            configuration.AddEnvironmentVariables();

            return configuration;
        }

        public static IServiceCollection RegisterAssemblies(this IServiceCollection services, IConfiguration configuration)
        {
            _configuration = configuration;
            _assemblies = ModuleLoader.LoadAssemblies(configuration, "CL.Module.");
            _modules = ModuleLoader.LoadModules(_assemblies);

            return services;
        }

        public static IServiceCollection RegisterModules(this IServiceCollection services)
        {
            services.AddModuleInfo(_modules);
            services.AddModuleRequests(_assemblies);

            foreach (var module in _modules)
            {
                module.Register(services, _configuration);
            }

            return services;
        }

        public static IServiceCollection RegisterDispatcher(this IServiceCollection services)
        {
            services.AddCommands(_assemblies);
            services.AddQueries(_assemblies);
            services.AddEvents(_assemblies);
            services.AddSingleton<IDispatcher, Dispatcher>();

            return services;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddMemoryCache();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddHttpClient();

            services.AddDistributedMemoryCache();
            services.AddSession();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMessaging(_configuration, _assemblies);
            services.AddErrorHandling();

            return services;
        }

        public static IServiceCollection RegisterAuthorization(this IServiceCollection services)
        {
            services.AddAuthentication(authentication =>
            {
                authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                authentication.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = _configuration["JwtSettings:Issuer"],
                    ValidAudience = _configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });

            services.AddAuthorization();
            services.RegisterAuth();

            return services;
        }

        public static WebApplication UseSwagger(this WebApplication application)
        {
            if (application.Environment.IsDevelopment())
            {
                SwaggerBuilderExtensions.UseSwagger(application);
                application.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Contact List API");
                });
            }

            return application;
        }

        public static WebApplication UseJwt(this WebApplication application)
        {
            application.UseSession();

            application.Use(async (context, next) =>
            {
                var JWToken = context.Session.GetString("JWToken");

                if (!string.IsNullOrEmpty(JWToken))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
                }

                await next();
            });

            application.UseAuthentication();
            application.UseAuthorization();

            return application;
        }

        public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : new()
            => configuration.GetSection(sectionName).GetOptions<T>();

        public static T GetOptions<T>(this IConfigurationSection section) where T : new()
        {
            var options = new T();
            section.Bind(options);
            return options;
        }

        public static string GetModuleName(this object value)
            => value?.GetType().GetModuleName() ?? string.Empty;

        public static string GetModuleName(this Type type, string namespacePart = "Module", int splitIndex = 2)
        {
            if (type?.Namespace is null)
            {
                return string.Empty;
            }

            return type.Namespace.Contains(namespacePart)
                ? type.Namespace.Split(".")[splitIndex].ToLowerInvariant()
                : string.Empty;
        }
    }
}
