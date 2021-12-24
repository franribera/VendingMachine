using System.Security.Claims;
using Duende.IdentityServer;
using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Features.Products.Purchase;

[Route("buy")]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[ApiController]
public class PurchaseProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public PurchaseProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "buyer")]
    public async Task<ActionResult<PurchaseProductResponse>> Put([FromBody] PurchaseProductRequest request, CancellationToken cancellationToken)
    {
        var userId = Convert.ToInt64(User.FindFirstValue(JwtClaimTypes.Id));

        request.UserId = userId;

        return await _mediator.Send(request, cancellationToken);
    }
}