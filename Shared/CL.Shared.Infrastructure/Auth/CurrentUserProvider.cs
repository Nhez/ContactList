using System.Security.Claims;
using CL.Shared.Abstractions.Auth;
using Microsoft.AspNetCore.Http;

namespace CL.Shared.Infrastructure.Auth;

public class CurrentUserProvider : ICurrentUserProvider
{
    private const string IdClaimKey = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
    private const string RoleClaimKey = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetId()
    {
        var user = GetClaimsPrincipal();

        if (user is null)
        {
            return Guid.Empty;
        }

        var claim = user.FindFirst(IdClaimKey);

        if (claim != null)
        {
            return Guid.Parse(claim.Value);
        }

        throw new KeyNotFoundException($"Missing claim: {IdClaimKey}");
    }

    public CurrentUserDto GetUser()
    {
        var claimsPrincipal = GetClaimsPrincipal();

        if (claimsPrincipal is null)
        {
            return null;
        }

        var dto = new CurrentUserDto(
            GetId(),
            claimsPrincipal.FindFirst("first_name").Value,
            claimsPrincipal.FindFirst("last_name").Value,
            GetRoles());

        return dto;
    }

    public void TryGetUser(out CurrentUserDto userDto)
    {
        TryGetClaimsPrincipal(out var claimsPrincipal);

        if (claimsPrincipal == null)
        {
            userDto = null;
            return;
        }

        userDto = new CurrentUserDto(
            GetId(),
            claimsPrincipal.FindFirst("first_name")?.Value ?? "",
            claimsPrincipal.FindFirst("last_name")?.Value ?? "",
            GetRoles());
    }

    public IEnumerable<string> GetRoles()
    {
        var user = GetClaimsPrincipal();
        if (user == null)
        {
            return Enumerable.Empty<string>();
        }

        var claims = user.FindAll(RoleClaimKey);

        return claims.Select(c => c.Value);
    }

    public bool HasRole(string roleName)
    {
        var roles = GetRoles();

        return roles.Any(r => r.Equals(roleName));
    }

    private ClaimsPrincipal GetClaimsPrincipal()
    {
        var user = _httpContextAccessor.HttpContext.User;
        if ((user != null) && user.Identity.IsAuthenticated)
        {
            return user;
        }

        return null;
    }

    private void TryGetClaimsPrincipal(out ClaimsPrincipal claimsPrincipal)
    {
        var user = _httpContextAccessor?.HttpContext?.User;
        if (user != null && user.Identity.IsAuthenticated)
        {
            claimsPrincipal = user;
            return;
        }

        claimsPrincipal = null;
    }
}