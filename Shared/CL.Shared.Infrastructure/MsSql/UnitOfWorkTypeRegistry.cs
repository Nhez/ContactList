namespace CL.Shared.Infrastructure.MsSql;

public class UnitOfWorkTypeRegistry
{
    private readonly Dictionary<string, Type> _types = new();

    public Type Resolve<T>() => _types.TryGetValue(GetKey<T>(), out var type) ? type : null;

    private static string GetKey<T>() => $"{typeof(T).GetModuleName()}";
}