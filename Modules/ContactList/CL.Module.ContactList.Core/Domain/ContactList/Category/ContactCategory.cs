using CL.Shared.Abstractions.Domain;
using CSharpFunctionalExtensions;

namespace CL.Module.ContactList.Core.Domain.ContactList.Category;

internal class ContactCategory : BaseEntity<int>
{
    private readonly List<ContactCategoryTranslation> _translations = new List<ContactCategoryTranslation>();

    private ContactCategory()
    {
        // Required By EF
    }

    private ContactCategory(bool isSubcategory, int? parentCategoryId)
    {
        IsSubcategory = isSubcategory;
        ParentCategoryId = parentCategoryId;
    }

    public bool IsSubcategory { get; private set; }
    public int? ParentCategoryId { get; private set; }
    public IReadOnlyCollection<ContactCategoryTranslation> Translations => _translations;

    public static ContactCategory Create(
        bool isSubcategory,
        int? parentCategoryId)
    {
        var category = new ContactCategory(isSubcategory, parentCategoryId);

        return category;
    }

    public Result AddTranslation(
        string languageCode,
        string value)
    {
        var isTranslationAlreadyAdded = _translations.Any(translation => translation.LanguageCode == languageCode);

        if (isTranslationAlreadyAdded)
        {
            return Result.Failure("translation-already-exists");
        }

        _translations.Add(ContactCategoryTranslation.Create(languageCode, value, Id));

        return Result.Success();
    }

}
