using CL.Module.ContactList.Application.Repositories;
using CL.Shared.Abstractions.Commands;
using CSharpFunctionalExtensions;
using JetBrains.Annotations;

namespace CL.Module.ContactList.Application.Commands.DeleteContact;

[UsedImplicitly]
internal sealed class DeleteContactCommandHandler(IContactListRepository repository)
    : IResultCommandHandler<DeleteContactCommand, Result>
{
    public async Task<Result> HandleAsync(DeleteContactCommand command, CancellationToken cancellationToken = default)
    {
        var person = await repository.GetContactByIdAsync(command.ContactId, cancellationToken);

        if (person is null)
        {
            return Result.Failure("person-not-found");
        }

        await repository.RemoveContactAsync(person, cancellationToken);

        return Result.Success();
    }
}