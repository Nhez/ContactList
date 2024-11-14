using CL.Shared.Abstractions.Contexts;

namespace CL.Shared.Abstractions.Messages.Contexts;

public interface IMessageContext
{
    public Guid MessageId { get; }
    public IContext Context { get; }
}
