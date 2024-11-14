using CL.Module.ContactList.Application.Dtos;
using CL.Module.ContactList.Application.Requests;
using CL.Shared.Abstractions.Commands;
using CSharpFunctionalExtensions;

namespace CL.Module.ContactList.Application.Commands.AddContact;

public class AddContactCommand : ICommand<Result<ContactDto>>
{
    public AddContactCommand(
        string name,
        string surname,
        string email,
        int categoryId,
        ContactSubcategoryRequest? subCategory,
        string phone,
        DateTimeOffset dateOfBirth)
    {
        Name = name;
        Surname = surname;
        Email = email;
        CategoryId = categoryId;
        SubCategory = subCategory;
        Phone = phone;
        DateOfBirth = dateOfBirth;
    }

    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public int CategoryId { get; set; }
    public ContactSubcategoryRequest? SubCategory { get; set; }
    public string Phone { get; set; }
    public DateTimeOffset DateOfBirth { get; set; }
}
