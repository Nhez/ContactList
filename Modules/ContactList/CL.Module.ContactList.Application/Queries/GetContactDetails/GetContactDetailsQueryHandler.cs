using CL.Module.ContactList.Application.Dtos;
using CL.Module.ContactList.Application.Queries.GetContactsQuery;
using CL.Module.ContactList.Application.Services;
using CL.Module.ContactList.Application.Storages;
using CL.Shared.Abstractions.CommonTypes;
using CL.Shared.Abstractions.Queries;
using JetBrains.Annotations;

namespace CL.Module.ContactList.Application.Queries.GetContactDetails;

[UsedImplicitly]
internal sealed class GetContactDetailsQueryHandler : IQueryHandler<GetContactDetailsQuery, ContactDetailsDto>
{
    private readonly IContactListStorage _contactListStorage;
    private readonly IContactListService _contactListService;

    public GetContactDetailsQueryHandler(
        IContactListStorage contactListStorage,
        IContactListService contactListService)
    {
        _contactListStorage = contactListStorage;
        _contactListService = contactListService;
    }

    public async Task<ContactDetailsDto> HandleAsync(GetContactDetailsQuery query, CancellationToken cancellationToken = default)
    {
        //var contactDetails = await _contactListStorage.GetContactDetailsDto(query.ContactId, _contactListService.GetLanguage(), cancellationToken);
        var contactDetails = GetContactDetails();

        return contactDetails;
    }

    private ContactDetailsDto GetContactDetails()
    {
        var contactsList = new ContactDetailsDto(
            6,
            "Filip",
            "Szósty",
            "FilipTestowy@Gmail.com",
            GetContactCategoryDto(1, ContactCategory.Buisness),
            GetContactCategoryDto(2, ContactCategory.Buisness),
            "123 456 789",
            DateTimeOffset.UtcNow);

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
