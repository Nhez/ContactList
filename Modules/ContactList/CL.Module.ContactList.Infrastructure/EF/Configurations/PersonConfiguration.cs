using CL.Module.ContactList.Core.Domain.ContactList;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CL.Module.ContactList.Infrastructure.EF.Configurations;

[UsedImplicitly]
internal class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.HasKey(x => x.Id);

        builder.ToTable("People");

        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Email).IsRequired();
        builder.Property(x => x.Phone).IsRequired();
        builder.Property(x => x.DateOfBirth).IsRequired();

        builder.HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId);

        builder.HasOne(x => x.SubCategory)
            .WithMany()
            .HasForeignKey(x => x.SubCategoryId);
    }
}