using Microsoft.AspNetCore.Authorization;

namespace CL.Shared.Infrastructure.Auth;

public class AuthorizeRolesAttribute : AuthorizeAttribute
{
    public AuthorizeRolesAttribute(params string[] roles)
    {
        {
            Roles = string.Join(",", roles);
        }
    }
}
