using CL.Shared.Abstractions.Domain;

namespace CL.Module.ContactList.Core.Domain.ContactList.Category;

internal class ContactCategoryTranslation : BaseEntity<int>
{
    private ContactCategoryTranslation()
    {
        // Required By EF
    }

    private ContactCategoryTranslation(
        string languageCode,
        string value,
        int categoryId)
    {
        LanguageCode = languageCode;
        Value = value;
        CategoryId = categoryId;
    }

    public string LanguageCode { get; private set; }
    public string Value { get; private set; }
    public int CategoryId { get; private set; }
    public ContactCategory Category { get; private set; }

    public static ContactCategoryTranslation Create(string languageCode,
        string value,
        int categoryId)
    {
        return new ContactCategoryTranslation(languageCode, value, categoryId);
    }
}
