namespace CL.Shared.Abstractions.Messages.Contexts;

public interface IMessageContextRegistry
{
    void Set(IMessage message, IMessageContext context);
}
