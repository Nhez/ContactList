﻿namespace CL.Shared.Abstractions.Exceptions;

public interface IExceptionCompositionRoot
{
    ExceptionResponse Map(Exception exception);
}
