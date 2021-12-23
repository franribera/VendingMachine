using Duende.IdentityServer;
using Duende.IdentityServer.Services;
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
    private readonly IPersistedGrantService _persistedGrantService;

    public DeleteUserController(IMediator mediator, IPersistedGrantService persistedGrantService)
    {
        _mediator = mediator;
        _persistedGrantService = persistedGrantService;
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(JwtClaimTypes.Id);
        var sub = User.FindFirstValue(JwtClaimTypes.Subject);
        var client = User.FindFirstValue(JwtClaimTypes.ClientId);

        var request = new DeleteUserRequest { UserId = Convert.ToInt64(userId) };

        await _mediator.Send(request, cancellationToken);

        await _persistedGrantService.RemoveAllGrantsAsync(sub, client);

        return Ok();
    }
}