namespace CL.Shared.Abstractions.Auth;

public class CurrentUserDto
{
    public CurrentUserDto(Guid id, string firstName, string lastName, IEnumerable<string> roles)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Roles = roles;
    }

    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public IEnumerable<string> Roles { get; set; }
    public string FullName() => $"{FirstName} {LastName}";
}
