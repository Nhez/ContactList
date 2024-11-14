using CL.Module.ContactList.Application.Dtos;
using CL.Module.ContactList.Application.Services;
using CL.Module.ContactList.Core.Domain.ContactList.Category;

namespace CL.Module.ContactList.Infrastructure.Services;

internal sealed class ContactListService : IContactListService
{
    private const string DefaultLanguage = "pl";

    public ContactCategoryDto GetCategoryDto(ContactCategory category, string language)
    {
        var translation = category.Translations
            .First(x => x.LanguageCode == language);

        var dto = new ContactCategoryDto(
            category.Id,
            language,
            translation.Value,
            category.ParentCategoryId);

        return dto;
    }

    public string GetLanguage()
    {
        return DefaultLanguage;
    }
}
