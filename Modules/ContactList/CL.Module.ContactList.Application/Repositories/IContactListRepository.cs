using CL.Module.ContactList.Core.Domain.ContactList;
using CL.Module.ContactList.Core.Domain.ContactList.Category;

namespace CL.Module.ContactList.Application.Repositories;

internal interface IContactListRepository
{
    public Task AddContactAsync(Person contact, CancellationToken cancellationToken = default);
    public Task UpdateContactAsync(Person contact, CancellationToken cancellationToken = default);
    public Task RemoveContactAsync(Person contact, CancellationToken cancellationToken = default);
    public Task<Person> GetContactByIdAsync(int contactId, CancellationToken cancellationToken = default);
    public Task AddContactCategoryAsync(ContactCategory category, CancellationToken cancellationToken = default);
}
