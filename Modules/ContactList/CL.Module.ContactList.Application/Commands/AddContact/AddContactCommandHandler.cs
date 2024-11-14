using CL.Module.ContactList.Application.Dtos;
using CL.Module.ContactList.Application.Repositories;
using CL.Module.ContactList.Application.Services;
using CL.Module.ContactList.Core.Domain.ContactList;
using CL.Module.ContactList.Core.Domain.ContactList.Category;
using CL.Shared.Abstractions.Commands;
using CSharpFunctionalExtensions;
using JetBrains.Annotations;

namespace CL.Module.ContactList.Application.Commands.AddContact;

[UsedImplicitly]
internal sealed class AddContactCommandHandler(IContactListRepository repository, IContactListService service)
    : IResultCommandHandler<AddContactCommand, Result<ContactDto>>
{
    public async Task<Result<ContactDto>> HandleAsync(AddContactCommand command, CancellationToken cancellationToken = default)
    {
        var subCategoryId = command.SubCategory?.Id;
        var isNewSubcategory = command.SubCategory is not null && !command.SubCategory.Id.HasValue;

        if (isNewSubcategory)
        {
            var subcategory = ContactCategory.Create(true, command.CategoryId);

            subcategory.AddTranslation(service.GetLanguage(), command.SubCategory.Value);

            await repository.AddContactCategoryAsync(subcategory, cancellationToken);

            subCategoryId = subcategory.Id;
        }

        var person = Person.Create(
            command.Name,
            command.Surname,
            command.Email,
            command.CategoryId,
            subCategoryId,
            command.Phone,
            command.DateOfBirth);

        await repository.AddContactAsync(person, cancellationToken);

        var dto = new ContactDto(
            person.Id,
            person.Name,
            person.Surname,
            service.GetCategoryDto(person.Category, service.GetLanguage()));

        return Result.Success(dto);
    }
}