using CL.Shared.Abstractions.Messages;
using CL.Shared.Abstractions.Messages.Contexts;

namespace CL.Shared.Infrastructure.Messages.Dispatchers;

public record MessageEnvelope(IMessage Message, IMessageContext MessageContext);