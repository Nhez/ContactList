using CL.Module.ContactList.Core.Domain.ContactList.Category;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CL.Module.ContactList.Infrastructure.EF.Configurations;

[UsedImplicitly]
internal class ContactCategoryConfiguration : IEntityTypeConfiguration<ContactCategory>
{
    public void Configure(EntityTypeBuilder<ContactCategory> builder)
    {
        builder.ToTable("ContactCategories");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.IsSubcategory)
            .IsRequired()
            .HasDefaultValue(value: false);
    }
}