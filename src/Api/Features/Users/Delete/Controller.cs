using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Api.Features.Users.Create;
using Duende.IdentityServer;
using Duende.IdentityServer.Services;
using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Users.Delete;

[Route("users")]
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

    [HttpDelete()]
    public async Task<IActionResult> Delete(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(JwtClaimTypes.Id);
        var sub = User.FindFirstValue(JwtClaimTypes.Subject);
        var client = User.FindFirstValue(JwtClaimTypes.ClientId);

        var request = new DeleteUserRequest { UserId = Convert.ToInt64(userId) };

        await _mediator.Send(request, cancellationToken);

        //await HttpContext.SignOutAsync();
        //await HttpContext.SignOutAsync(IdentityServerConstants.LocalApi.AuthenticationScheme);

        await _persistedGrantService.RemoveAllGrantsAsync(sub, client);

        return Ok();
    }
}