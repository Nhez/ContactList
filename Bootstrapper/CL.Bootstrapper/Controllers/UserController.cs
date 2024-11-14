using Microsoft.AspNetCore.Mvc;
using CL.Shared.Abstractions.Auth;
using CL.Shared.Abstractions.Dispatchers;
using CL.Shared.Infrastructure.Controllers;

namespace CL.Bootstrapper.Controllers;

public class UserController : BaseController
{
    private readonly IConfiguration _configuration;

    public UserController(
        IDispatcher dispatcher,
        ICurrentUserProvider currentUserProvider,
        IConfiguration configuration)
        : base(dispatcher, currentUserProvider)
    {
        _configuration = configuration;
    }

    [HttpPost("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest, CancellationToken cancellationToken = default)
    {
        var tokenGenerator = new TokenGenerator(_configuration);
        var accessToken = tokenGenerator.GenerateToken(loginRequest.Username);

        HttpContext.Session.SetString("JWToken", accessToken);

        return Ok(accessToken);
    }

    [HttpPost("[action]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken = default)
    {
        HttpContext.Session.Clear();

        return Ok();
    }
}
