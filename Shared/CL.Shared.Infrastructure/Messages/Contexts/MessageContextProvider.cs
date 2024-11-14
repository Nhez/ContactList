using CL.Shared.Abstractions.Messages;
using CL.Shared.Abstractions.Messages.Contexts;
using Microsoft.Extensions.Caching.Memory;

namespace CL.Shared.Infrastructure.Messages.Contexts;

public class MessageContextProvider : IMessageContextProvider
{
    private readonly IMemoryCache _cache;

    public MessageContextProvider(IMemoryCache cache)
    {
        _cache = cache;
    }

    public IMessageContext Get(IMessage message) => _cache.Get<IMessageContext>(message);
}