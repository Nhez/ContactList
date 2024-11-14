namespace CL.Shared.Abstractions.Exceptions;

public interface IExceptionResponseMapper
{
    ExceptionResponse Map(Exception exception);
}
