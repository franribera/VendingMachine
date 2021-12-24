using Duende.IdentityServer;
using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Features.Products.Delete;

[Route("products")]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[ApiController]
public class DeleteProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public DeleteProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "seller")]
    public async Task<IActionResult> Delete([FromRoute] long id, CancellationToken cancellationToken)
    {
        var userId = Convert.ToInt64(User.FindFirstValue(JwtClaimTypes.Id));

        var request = new DeleteProductRequest { ProductId = id, UserId = userId };

        await _mediator.Send(request, cancellationToken);

        return Ok();
    }
}