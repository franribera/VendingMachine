using Duende.IdentityServer;
using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Features.Users.Read;

[Route("user")]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[ApiController]
public class ReadUserController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReadUserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ReadUserResponse>> Get(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(JwtClaimTypes.Id);

        var request = new ReadUserRequest { UserId = Convert.ToInt64(userId) };

        var response = await _mediator.Send(request, cancellationToken);

        return Ok(response);
    }
}