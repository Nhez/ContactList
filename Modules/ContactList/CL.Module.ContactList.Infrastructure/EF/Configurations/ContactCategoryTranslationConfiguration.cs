using CL.Module.ContactList.Core.Domain.ContactList.Category;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CL.Module.ContactList.Infrastructure.EF.Configurations;

[UsedImplicitly]
internal class ContactCategoryTranslationConfiguration : IEntityTypeConfiguration<ContactCategoryTranslation>
{
    public void Configure(EntityTypeBuilder<ContactCategoryTranslation> builder)
    {
        builder.ToTable("ContactCategoriesTranslations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.LanguageCode)
            .IsRequired()
            .HasMaxLength(maxLength: 2);

        builder.Property(x => x.Value)
            .IsRequired();

        builder.HasOne(x => x.Category)
            .WithMany(x => x.Translations)
            .OnDelete(DeleteBehavior.Cascade);

    }
}