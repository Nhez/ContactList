using CL.Module.ContactList.Core.Domain.ContactList;
using CL.Module.ContactList.Core.Domain.ContactList.Category;
using Microsoft.EntityFrameworkCore;

namespace CL.Module.ContactList.Infrastructure.EF;

internal sealed class ContactListContext : DbContext
{
    public ContactListContext(DbContextOptions<ContactListContext> options) : base(options)
    {
    }

    public DbSet<Person> People { get; set; }
    public DbSet<ContactCategory> ContactCategories{ get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContactListContext).Assembly);
        modelBuilder.HasDefaultSchema("ContactList");
    }
}