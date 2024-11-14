namespace CL.Shared.Abstractions.Time;

public interface IClock
{
    DateTimeOffset CurrentDate();
}