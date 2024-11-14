using CL.Module.ContactList.Application.Dtos;
using CL.Module.ContactList.Application.Repositories;
using CL.Module.ContactList.Application.Services;
using CL.Module.ContactList.Core.Domain.ContactList.Category;
using CL.Shared.Abstractions.Commands;
using CSharpFunctionalExtensions;
using JetBrains.Annotations;

namespace CL.Module.ContactList.Application.Commands.EditContact;

[UsedImplicitly]
internal sealed class EditContactCommandHandler(IContactListRepository repository, IContactListService service)
    : IResultCommandHandler<EditContactCommand, Result<ContactDto>>
{
    public async Task<Result<ContactDto>> HandleAsync(EditContactCommand command, CancellationToken cancellationToken = default)
    {
        var person = await repository.GetContactByIdAsync(command.ContactId, cancellationToken);

        if (person is null)
        {
            return Result.Failure<ContactDto>("person-not-found");
        }

        var subCategoryId = command.SubCategory?.Id;
        var isNewSubcategory = command.SubCategory is not null && !command.SubCategory.Id.HasValue;

        if (isNewSubcategory)
        {
            var subcategory = ContactCategory.Create(true, command.CategoryId);

            subcategory.AddTranslation(service.GetLanguage(), command.SubCategory.Value);

            await repository.AddContactCategoryAsync(subcategory, cancellationToken);

            subCategoryId = subcategory.Id;
        }

        person.Edit(
            command.Name,
            command.Surname,
            command.Email,
            command.CategoryId,
            subCategoryId,
            command.Phone,
            command.DateOfBirth);

        await repository.UpdateContactAsync(person, cancellationToken);

        var dto = new ContactDto(
            person.Id,
            person.Name,
            person.Surname,
            service.GetCategoryDto(person.Category, service.GetLanguage()));

        return Result.Success(dto);
    }
}