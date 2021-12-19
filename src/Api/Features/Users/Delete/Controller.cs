using Api.Features.Users.Create;
using Duende.IdentityServer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Users.Delete;

[Route("users")]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
public class DeleteUserController : Controller
{
    private readonly IMediator _mediator;

    public DeleteUserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<CreateUserResponse>> Delete([FromRoute] long id, CancellationToken cancellationToken)
    {
        var request = new DeleteUserRequest { UserId = id };

        await _mediator.Send(request, cancellationToken);

        return Ok();
    }
}