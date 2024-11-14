using CL.Shared.Infrastructure;

namespace CL.Bootstrapper;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.RegisterConfiguration(typeof(Program));

        builder.Services.RegisterAssemblies(builder.Configuration);
        builder.Services.RegisterModules();
        builder.Services.RegisterDispatcher();
        builder.Services.RegisterServices();
        builder.Services.RegisterAuthorization();

        var app = builder.Build();

        app.UseDefaultFiles();
        app.UseStaticFiles();
        app.UseSwagger();
        app.UseHttpsRedirection();

        app.UseJwt();

        app.MapControllers();
        app.MapFallbackToFile("/index.html");

        app.Run();
    }
}
