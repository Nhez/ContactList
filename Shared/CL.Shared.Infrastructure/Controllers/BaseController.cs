using CL.Shared.Abstractions.Auth;
using CL.Shared.Abstractions.Dispatchers;
using Microsoft.AspNetCore.Mvc;

namespace CL.Shared.Infrastructure.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class BaseController : ControllerBase
{
    protected ICurrentUserProvider CurrentUserProvider;
    protected IDispatcher Dispatcher;

    public BaseController(IDispatcher dispatcher, ICurrentUserProvider currentUserProvider)
    {
        Dispatcher = dispatcher;
        CurrentUserProvider = currentUserProvider;
    }
}