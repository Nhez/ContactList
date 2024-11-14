using CL.Module.ContactList.Application.Dtos;
using CL.Module.ContactList.Core.Domain.ContactList;

namespace CL.Module.ContactList.Application.Storages;

internal interface IContactListStorage
{
    public Task<List<Person>> GetContacts(CancellationToken cancellationToken = default);
    public Task<List<ContactDto>> GetContactsDtos(string language, CancellationToken cancellationToken = default);
    public Task<List<ContactCategoryDto>> GetContactCategoriesDtos(string language, CancellationToken cancellationToken = default);
    public Task<ContactDetailsDto> GetContactDetailsDto(int contactId, string language, CancellationToken cancellationToken = default);
}
