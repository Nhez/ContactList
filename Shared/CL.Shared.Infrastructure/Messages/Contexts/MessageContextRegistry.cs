using CL.Shared.Abstractions.Messages;
using CL.Shared.Abstractions.Messages.Contexts;
using Microsoft.Extensions.Caching.Memory;

namespace CL.Shared.Infrastructure.Messages.Contexts;

public class MessageContextRegistry : IMessageContextRegistry
{
    private readonly IMemoryCache _cache;

    public MessageContextRegistry(IMemoryCache cache)
    {
        _cache = cache;
    }

    public void Set(IMessage message, IMessageContext context)
        => _cache.Set(message, context, new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(1)
        });
}