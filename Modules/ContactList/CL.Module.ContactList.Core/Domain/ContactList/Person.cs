using CL.Shared.Abstractions.Domain;
using CL.Module.ContactList.Core.Domain.ContactList.Category;

namespace CL.Module.ContactList.Core.Domain.ContactList;

internal class Person : BaseEntity<int>
{
    private Person()
    {
        // Required By EF
    }

    private Person(
        string name,
        string surname,
        string email,
        int categoryId,
        int? subCategoryId,
        string phone,
        DateTimeOffset dateOfBirth)
    {
        Name = name;
        Surname = surname;
        Email = email;
        CategoryId = categoryId;
        SubCategoryId = subCategoryId;
        Phone = phone;
        DateOfBirth = dateOfBirth;
    }

    public string Name { get; private set; }
    public string Surname { get; private set; }
    public string Email { get; private set; }
    public int CategoryId { get; private set; }
    public int? SubCategoryId { get; private set; }
    public string Phone { get; private set; }
    public DateTimeOffset DateOfBirth { get; private set; }
    public ContactCategory Category { get; private set; }
    public ContactCategory SubCategory { get; private set; }

    public static Person Create(
        string name,
        string surname,
        string email,
        int categoryId,
        int? subCategoryId,
        string phone,
        DateTimeOffset dateOfBirth)
    {
        var person = new Person(
            name,
            surname,
            email,
            categoryId,
            subCategoryId,
            phone,
            dateOfBirth);

        return person;
    }

    public void Edit(
        string name,
        string surname,
        string email,
        int categoryId,
        int? subCategoryId,
        string phone,
        DateTimeOffset dateOfBirth)
    {
        Name = name;
        Surname = surname;
        Email = email;
        CategoryId = categoryId;
        SubCategoryId = subCategoryId;
        Phone = phone;
        DateOfBirth = dateOfBirth;
    }
}
