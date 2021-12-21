using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Api.Features.Users.Create;
using Duende.IdentityServer;
using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Users.Delete;

[Route("users")]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[ApiController]
public class DeleteUserController : ControllerBase
{
    private readonly IMediator _mediator;

    public DeleteUserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(JwtClaimTypes.Id);

        if (string.IsNullOrWhiteSpace(userId) || id.ToString() != userId) return Forbid();
        
        var request = new DeleteUserRequest { UserId = id };

        await _mediator.Send(request, cancellationToken);

        return Ok();
    }
}