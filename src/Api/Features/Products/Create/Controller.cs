using System.Security.Claims;
using Duende.IdentityServer;
using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Products.Create;

[Route("products")]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[ApiController]
public class CreateProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public CreateProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "seller")]
    public async Task<ActionResult<CreateProductResponse>> Post([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        var userId = Convert.ToInt64(User.FindFirstValue(JwtClaimTypes.Id));

        request.UserId = userId;

        return await _mediator.Send(request, cancellationToken);
    }
}