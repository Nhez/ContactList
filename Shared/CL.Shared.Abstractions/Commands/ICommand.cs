using CL.Shared.Abstractions.Messages;

namespace CL.Shared.Abstractions.Commands;

public interface ICommand : IMessage
{
}

public interface ICommand<T> : ICommand
{
}
