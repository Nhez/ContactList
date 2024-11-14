namespace CL.Module.ContactList.Application.Dtos;

public record ContactCategoryDto(
    int Id,
    string Language,
    string Value,
    int? parentCategoryId);
