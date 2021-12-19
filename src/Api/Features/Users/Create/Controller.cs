using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Users.Create;

[AllowAnonymous]
[Route("users")]
public class CreateUserController : Controller
{
    private readonly IMediator _mediator;

    public CreateUserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<CreateUserResponse>> Post([FromBody]CreateUserRequest request, CancellationToken cancellationToken)
    {
        return await _mediator.Send(request, cancellationToken);
    }
}