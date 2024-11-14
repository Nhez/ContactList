namespace CL.Shared.Abstractions.Messages.Brokers;

public interface IMessageBroker
{
    Task PublishAsync(IMessage message, CancellationToken cancellationToken = default);
}
