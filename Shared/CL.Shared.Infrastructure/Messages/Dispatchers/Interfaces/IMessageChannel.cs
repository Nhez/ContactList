using System.Threading.Channels;

namespace CL.Shared.Infrastructure.Messages.Dispatchers.Interfaces;

public interface IMessageChannel
{
    ChannelReader<MessageEnvelope> Reader { get; }
    ChannelWriter<MessageEnvelope> Writer { get; }
}