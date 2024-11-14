namespace CL.Module.ContactList.Application.Dtos;

public record ContactDto(
    int Id,
    string Name,
    string Surname,
    ContactCategoryDto Category);
