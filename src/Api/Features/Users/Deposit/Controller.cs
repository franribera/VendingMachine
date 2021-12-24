using System.Security.Claims;
using Duende.IdentityServer;
using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Users.Deposit;

[Route("deposit")]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[ApiController]
public class DepositController : ControllerBase
{
    private readonly IMediator _mediator;

    public DepositController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "buyer")]
    public async Task<ActionResult<DepositResponse>> Post([FromBody] DepositRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(JwtClaimTypes.Id);

        request.UserId = Convert.ToInt64(userId);

        return await _mediator.Send(request, cancellationToken);
    }
}