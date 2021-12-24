using System.Security.Claims;
using Duende.IdentityServer;
using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Users.Reset;

[Route("reset")]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[ApiController]
public class ResetController : ControllerBase
{
    private readonly IMediator _mediator;

    public ResetController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "buyer")]
    public async Task<ActionResult<ResetResponse>> Post(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(JwtClaimTypes.Id);

        var request = new ResetRequest { UserId = Convert.ToInt64(userId) };

        return await _mediator.Send(request, cancellationToken);
    }
}