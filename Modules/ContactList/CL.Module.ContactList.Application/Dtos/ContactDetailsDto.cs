namespace CL.Module.ContactList.Application.Dtos;

public record ContactDetailsDto(
    int Id,
    string Name,
    string Surname,
    string Email,
    ContactCategoryDto Category,
    ContactCategoryDto? SubCategory,
    string Phone,
    DateTimeOffset DateOfBirth);
