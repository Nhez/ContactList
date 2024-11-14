namespace CL.Shared.Abstractions.MsSql;

public interface IUnitOfWork
{
    Task ExecuteAsync(Func<Task> action);
    Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> action);
}
