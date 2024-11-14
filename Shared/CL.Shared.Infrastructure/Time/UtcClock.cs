using CL.Shared.Abstractions.Time;

namespace CL.Shared.Infrastructure.Time;

public class UtcClock : IClock
{
    public DateTimeOffset CurrentDate() => DateTimeOffset.UtcNow;
}