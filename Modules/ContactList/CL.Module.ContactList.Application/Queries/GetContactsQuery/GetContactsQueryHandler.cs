using CL.Module.ContactList.Application.Dtos;
using CL.Module.ContactList.Application.Services;
using CL.Module.ContactList.Application.Storages;
using CL.Shared.Abstractions.CommonTypes;
using CL.Shared.Abstractions.Queries;
using JetBrains.Annotations;

namespace CL.Module.ContactList.Application.Queries.GetContactsQuery;

[UsedImplicitly]
internal sealed class GetContactsQueryHandler : IQueryHandler<GetContactsQuery, List<ContactDto>>
{
    private readonly IContactListStorage _contactListStorage;
    private readonly IContactListService _contactListService;

    public GetContactsQueryHandler(
        IContactListStorage contactListStorage,
        IContactListService contactListService)
    {
        _contactListStorage = contactListStorage;
        _contactListService = contactListService;
    }

    public async Task<List<ContactDto>> HandleAsync(GetContactsQuery query, CancellationToken cancellationToken = default)
    {
        //var contacts = await _contactListStorage.GetContactsDtos(_contactListService.GetLanguage(), cancellationToken);
        var contacts = GetListOfContacts();

        return contacts;
    }

    private List<ContactDto> GetListOfContacts()
    {
        var contactsList = new List<ContactDto>
        {
            new ContactDto(1, "Antoni", "Pierwszy", GetContactCategoryDto(1, ContactCategory.Buisness)),
            new ContactDto(2, "Bogdan", "Drugi", GetContactCategoryDto(2, ContactCategory.Private)),
            new ContactDto(3, "Cecylia", "Trzecia", GetContactCategoryDto(3, ContactCategory.Buisness)),
            new ContactDto(4, "Danuta", "Czwarta", GetContactCategoryDto(4, ContactCategory.Private)),
            new ContactDto(5, "Eustachy", "Piąty", GetContactCategoryDto(5, ContactCategory.Other))
        };

        return contactsList;
    }
    private ContactCategoryDto GetContactCategoryDto(int categoryId, ContactCategory category)
    {
        var type = category switch
        {
            ContactCategory.Private => "Private",
            ContactCategory.Buisness => "Buisness",
            _ => "Other"
        };

        var result = new ContactCategoryDto(
            categoryId,
            _contactListService.GetLanguage(),
            type,
            null);

        return result;
    }
}
