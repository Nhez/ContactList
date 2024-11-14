using CL.Module.ContactList.Application.Dtos;
using CL.Module.ContactList.Core.Domain.ContactList.Category;

namespace CL.Module.ContactList.Application.Services;

internal interface IContactListService
{
    ContactCategoryDto GetCategoryDto(ContactCategory category, string language);
    string GetLanguage();
}
