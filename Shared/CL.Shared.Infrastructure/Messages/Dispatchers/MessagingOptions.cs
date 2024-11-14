namespace CL.Shared.Infrastructure.Messages.Dispatchers;

public class MessagingOptions
{
    public bool UseAsyncDispatcher { get; set; }
    public bool UseRabbitMq { get; set; } = true;
}