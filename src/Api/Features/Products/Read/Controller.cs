using Api.Infrastructure.Persistence;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Products.Read;

[Route("products")]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[ApiController]
public class UpdateProductController : ControllerBase
{
    private readonly VendingMachineDbContext _dbContext;

    public UpdateProductController(VendingMachineDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    [HttpGet("{productId:long}")]
    public async Task<ActionResult<ProductDto>> GetSingle([FromRoute] long productId, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.SingleOrDefaultAsync(p => p.Id == productId, cancellationToken);

        if (product == null) return NotFound();

        return Ok(new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price.Amount,
            Quantity = product.Quantity,
            SellerId = product.SellerId
        });
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll(CancellationToken cancellationToken)
    {
        return await _dbContext.Products
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price.Amount,
                Quantity = p.Quantity,
                SellerId = p.SellerId
            })
            .ToListAsync(cancellationToken);
    }
}

public class ProductDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public long SellerId { get; set; }
    public int Quantity { get; set; }
    public int Price { get; set; }
}