namespace CL.Module.ContactList.Application.Requests;

public record AddContactRequest(
    string Name,
    string Surname,
    string Email,
    int CategoryId,
    ContactSubcategoryRequest? SubCategory,
    string Phone,
    DateTimeOffset DateOfBirth);