using CL.Module.ContactList.Application.Dtos;
using CL.Shared.Abstractions.Queries;

namespace CL.Module.ContactList.Application.Queries.GetContactsQuery;

public class GetContactDetailsQuery : IQuery<ContactDetailsDto>
{
    public GetContactDetailsQuery(int contactId)
    {
        ContactId = contactId;
    }

    public int ContactId { get; set; }
}
