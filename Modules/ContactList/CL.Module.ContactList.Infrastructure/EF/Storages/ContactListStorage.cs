using CL.Module.ContactList.Application.Dtos;
using CL.Module.ContactList.Application.Services;
using CL.Module.ContactList.Application.Storages;
using CL.Module.ContactList.Core.Domain.ContactList;
using CL.Module.ContactList.Core.Domain.ContactList.Category;
using Microsoft.EntityFrameworkCore;

namespace CL.Module.ContactList.Infrastructure.EF.Storages;

internal sealed class ContactListStorage : IContactListStorage
{
    private readonly IQueryable<Person> _people;
    private readonly IQueryable<ContactCategory> _categories;
    private readonly IContactListService _contactListService;

    public ContactListStorage(
        ContactListContext context,
        IContactListService contactListService)
    {
        _people = context.People.AsNoTracking();
        _categories = context.ContactCategories.AsNoTracking();
        _contactListService = contactListService;
    }

    public async Task<List<Person>> GetContacts(CancellationToken cancellationToken = default)
    {
        var people = await _people.ToListAsync(cancellationToken);

        return people
            .OrderByDescending(contact => contact.Name)
            .ThenBy(contact => contact.Surname)
            .ToList();
    }

    public async Task<List<ContactDto>> GetContactsDtos(string language, CancellationToken cancellationToken = default)
    {
        var people = await _people.ToListAsync(cancellationToken);

        return people
            .Select(contact => new ContactDto(
                contact.Id,
                contact.Name,
                contact.Surname,
                _contactListService.GetCategoryDto(contact.Category, language)))
            .OrderByDescending(contact => contact.Name)
            .ThenBy(contact => contact.Surname)
            .ToList();
    }

    public async Task<List<ContactCategoryDto>> GetContactCategoriesDtos(string language, CancellationToken cancellationToken = default)
    {
        var categories = await _categories
            .Include(category => category.Translations)
            .ToListAsync(cancellationToken);

        return categories
            .Select(category => _contactListService.GetCategoryDto(category, language))
            .OrderByDescending(contact => contact.Value)
            .ToList();
    }

    public async Task<ContactDetailsDto> GetContactDetailsDto(int contactId, string language, CancellationToken cancellationToken = default)
    {
        var contact = await _people
            .Where(contact => contact.Id == contactId)
            .FirstOrDefaultAsync(cancellationToken);

        if (contact is null)
        {
            throw new ArgumentNullException(nameof(contact));
        }

        var contactDetailsDto = new ContactDetailsDto(
            contact.Id,
            contact.Name,
            contact.Surname,
            contact.Email,
            _contactListService.GetCategoryDto(contact.Category, language),
            contact.SubCategory is null ? null : _contactListService.GetCategoryDto(contact.SubCategory, language),
            contact.Phone,
            contact.DateOfBirth);

        return contactDetailsDto;
    }
}