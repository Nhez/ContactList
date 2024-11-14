namespace CL.Shared.Abstractions.Auth;

public interface ICurrentUserProvider
{
    Guid GetId();
    bool HasRole(string roleName);
    IEnumerable<string> GetRoles();
    CurrentUserDto GetUser();
    void TryGetUser(out CurrentUserDto userDto);
}
