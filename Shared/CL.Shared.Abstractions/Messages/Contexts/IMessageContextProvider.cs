namespace CL.Shared.Abstractions.Messages.Contexts;

public interface IMessageContextProvider
{
    IMessageContext Get(IMessage message);
}