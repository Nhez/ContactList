namespace CL.Module.ContactList.Application.Requests;

public record EditContactRequest(
    int ContactId,
    string Name,
    string Surname,
    string Email,
    int CategoryId,
    ContactSubcategoryRequest? SubCategory,
    string Phone,
    DateTimeOffset DateOfBirth);