using System.Threading.Channels;
using CL.Shared.Infrastructure.Messages.Dispatchers.Interfaces;

namespace CL.Shared.Infrastructure.Messages.Dispatchers;

public sealed class MessageChannel : IMessageChannel
{
    private readonly Channel<MessageEnvelope> _messages = Channel.CreateUnbounded<MessageEnvelope>();

    public ChannelReader<MessageEnvelope> Reader => _messages.Reader;
    public ChannelWriter<MessageEnvelope> Writer => _messages.Writer;
}