using System.Security.Claims;
using Duende.IdentityServer;
using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Products.Update;

[Route("products")]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[ApiController]
public class UpdateProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public UpdateProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut]
    [Authorize(Roles = "seller")]
    public async Task<ActionResult<UpdateProductResponse>> Put([FromBody] UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var userId = Convert.ToInt64(User.FindFirstValue(JwtClaimTypes.Id));

        request.UserId = userId;

        return await _mediator.Send(request, cancellationToken);
    }
}