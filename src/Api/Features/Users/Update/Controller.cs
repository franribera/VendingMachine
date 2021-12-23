using Duende.IdentityServer;
using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Features.Users.Update;

[Route("user")]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[ApiController]
public class UpdateUserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UpdateUserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut]
    public async Task<ActionResult<UpdateUserResponse>> Delete(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(JwtClaimTypes.Id);

        request.UserId = Convert.ToInt64(userId);

        var response = await _mediator.Send(request, cancellationToken);

        return Ok(response);
    }
}