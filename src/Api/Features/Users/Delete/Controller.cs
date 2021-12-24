using Duende.IdentityServer;
using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Api.Domain.Enumerations;
using Api.Features.Users.Reset;

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
        var userId = Convert.ToInt64(User.FindFirstValue(JwtClaimTypes.Id));
        var userRole = User.FindFirstValue(JwtClaimTypes.Role);

        var isBuyer = string.Equals(userRole, Role.Buyer.Name, StringComparison.InvariantCultureIgnoreCase);

        ResetResponse? response = null;

        if (isBuyer)
        {
            var resetRequest = new ResetRequest { UserId = userId };

            response = await _mediator.Send(resetRequest, cancellationToken);
        }
        
        var deleteUserRequest = new DeleteUserRequest { UserId = userId };

        await _mediator.Send(deleteUserRequest, cancellationToken);

        return response == null ? Ok() : new JsonResult(response);
    }
}