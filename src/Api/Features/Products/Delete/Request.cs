using Api.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Products.Delete;

public class DeleteProductRequest : IRequest
{
    public long UserId { get; set; }
    public long ProductId { get; set; }
}

public class DeleteProductRequestHandler : AsyncRequestHandler<DeleteProductRequest>
{
    private readonly VendingMachineDbContext _dbContext;

    public DeleteProductRequestHandler(VendingMachineDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override async Task Handle(DeleteProductRequest request, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.SingleOrDefaultAsync(p => p.Id == request.ProductId && p.SellerId == request.UserId, cancellationToken);

        if (product == null)
            throw new KeyNotFoundException($"Product id {request.ProductId} does not exist or you don't have rights to remove it.");

        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}