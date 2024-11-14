using System.Reflection;
using CL.Shared.Abstractions.Modules;
using Microsoft.Extensions.Configuration;

namespace CL.Shared.Infrastructure.Modules;

public static class ModuleLoader
{
    public static IList<Assembly> LoadAssemblies(IConfiguration configuration, string modulePart)
    {
        var assem = AppDomain.CurrentDomain.GetAssemblies();
        var assemblies = assem.Where(a => a.FullName.Contains(modulePart)).ToList();
        var locations = assemblies.Where(x => !x.IsDynamic).Select(x => x.Location).ToArray();
        var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
            .Where(x => !locations.Contains(x, StringComparer.InvariantCultureIgnoreCase))
            .Where(s => s.Contains(modulePart))
            .ToList();

        var disabledModules = new List<string>();

        foreach (var file in files)
        {
            if (!file.Contains(modulePart))
            {
                continue;
            }

            var moduleName = file.Split(modulePart)[1].Split(".")[0].ToLowerInvariant();
            var disabled = configuration.GetValue<bool>($"{moduleName}:module:disabled");

            if (disabled)
            {
                disabledModules.Add(file);
            }
        }

        foreach (var disabledModule in disabledModules)
        {
            files.Remove(disabledModule);
        }

        files.ForEach(x => assemblies.Add(Assembly.LoadFrom(x)));

        return assemblies;
    }

    public static IList<IModule> LoadModules(IEnumerable<Assembly> assemblies)
        => assemblies
            .Where(x => x.FullName.Contains("CL.Module"))
            .SelectMany(x => x.GetTypes())
            .Where(x => typeof(IModule).IsAssignableFrom(x) && !x.IsInterface)
            .OrderBy(x => x.Name)
            .Select(Activator.CreateInstance)
            .Cast<IModule>()
            .ToList();
}