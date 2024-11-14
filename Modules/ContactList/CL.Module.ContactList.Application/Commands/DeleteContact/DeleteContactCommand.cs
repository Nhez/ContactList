using CL.Shared.Abstractions.Commands;
using CSharpFunctionalExtensions;

namespace CL.Module.ContactList.Application.Commands.DeleteContact;

public class DeleteContactCommand : ICommand<Result>
{
    public DeleteContactCommand(int contactId)
    {
        ContactId = contactId;
    }

    public int ContactId { get; set; }
}
