using CL.Shared.Abstractions.Auth;

namespace CL.Shared.Abstractions.Contexts;

public interface IContext
{
    Guid RequestId { get; }
    Guid CorrelationId { get; }
    string TraceId { get; }
    string IpAddress { get; }
    string UserAgent { get; }
    CurrentUserDto ContextUser { get; }
}