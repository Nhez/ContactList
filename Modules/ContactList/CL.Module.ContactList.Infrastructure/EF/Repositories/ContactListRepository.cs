using CL.Module.ContactList.Application.Repositories;
using CL.Module.ContactList.Core.Domain.ContactList;
using CL.Module.ContactList.Core.Domain.ContactList.Category;
using Microsoft.EntityFrameworkCore;

namespace CL.Module.ContactList.Infrastructure.EF.Repositories;

internal sealed class ContactListRepository : IContactListRepository
{
    private readonly ContactListContext _context;
    private readonly DbSet<Person> _contacts;
    private readonly DbSet<ContactCategory> _categories;

    public ContactListRepository(ContactListContext context)
    {
        _context = context;
        _contacts = context.People;
        _categories = context.ContactCategories;
    }

    public async Task AddContactAsync(Person person, CancellationToken cancellationToken = default)
    {
        await _contacts.AddAsync(person, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateContactAsync(Person person, CancellationToken cancellationToken = default)
    {
        _contacts.Update(person);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveContactAsync(Person person, CancellationToken cancellationToken = default)
    {
        _contacts.Remove(person);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Person> GetContactByIdAsync(int contactId, CancellationToken cancellationToken = default)
    {
        return await _contacts.FirstOrDefaultAsync(contact => contact.Id == contactId, cancellationToken);
    }

    public async Task AddContactCategoryAsync(ContactCategory category, CancellationToken cancellationToken = default)
    {
        await _categories.AddAsync(category, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}