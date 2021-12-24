using Duende.IdentityServer;
using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Features.Users.Delete;

[Route("user")]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[ApiController]
public class DeleteUserController : ControllerBase
{
    private readonly IMediator _mediator;

    public DeleteUserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(JwtClaimTypes.Id);

        var request = new DeleteUserRequest { UserId = Convert.ToInt64(userId) };

        await _mediator.Send(request, cancellationToken);

        return Ok();
    }
}