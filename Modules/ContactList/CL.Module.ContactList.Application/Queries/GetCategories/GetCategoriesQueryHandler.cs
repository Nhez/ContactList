using CL.Module.ContactList.Application.Dtos;
using CL.Module.ContactList.Application.Queries.GetCategories;
using CL.Module.ContactList.Application.Services;
using CL.Module.ContactList.Application.Storages;
using CL.Shared.Abstractions.Queries;
using JetBrains.Annotations;

namespace CL.Module.ContactList.Application.Queries.GetContactsQuery;

[UsedImplicitly]
internal sealed class GetCategoriesQueryHandler : IQueryHandler<GetCategoriesQuery, List<ContactCategoryDto>>
{
    private readonly IContactListStorage _contactListStorage;
    private readonly IContactListService _contactListService;

    public GetCategoriesQueryHandler(
        IContactListStorage contactListStorage,
        IContactListService contactListService)
    {
        _contactListStorage = contactListStorage;
        _contactListService = contactListService;
    }

    public async Task<List<ContactCategoryDto>> HandleAsync(GetCategoriesQuery query, CancellationToken cancellationToken = default)
    {
        //var categories = await _contactListStorage.GetContactCategoriesDtos(_contactListService.GetLanguage(), cancellationToken);
        var categories = GetListOfCategories();

        return categories;
    }

    private List<ContactCategoryDto> GetListOfCategories()
    {
        var categories = new List<ContactCategoryDto>
        {
            new ContactCategoryDto(1, "pl", "Służbowy", null),
            new ContactCategoryDto(2, "pl", "Prywatny", null),
            new ContactCategoryDto(3, "pl", "Inny", 0),
            new ContactCategoryDto(4, "pl", "Szef", 1),
            new ContactCategoryDto(5, "pl", "Klient", 1),
        };

        return categories;
    }
}
