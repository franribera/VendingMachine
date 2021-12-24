using Api.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Products.Purchase;

public class PurchaseProductRequest : IRequest<PurchaseProductResponse>
{
    public long UserId { get; set; }
    public long ProductId { get; set; }
    public int Quantity { get; set; }
}

public class PurchaseProductResponse
{
    public long ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public int Cost { get; set; }
    public int Change { get; set; }
    public IEnumerable<int> Coins { get; set; }
}

public class PurchaseProductRequestHandler : IRequestHandler<PurchaseProductRequest, PurchaseProductResponse>
{
    private readonly VendingMachineDbContext _dbContext;

    public PurchaseProductRequestHandler(VendingMachineDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PurchaseProductResponse> Handle(PurchaseProductRequest request, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.SingleOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);
        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (product == null)
            throw new KeyNotFoundException($"Product id {request.ProductId} does not exist.");

        if (user == null)
            throw new ApplicationException("Your account has been deleted.");

        var cost = product.Take(request.Quantity);

        user.Withdraw(cost);

        var change = user.ResetDeposit();

        var response = new PurchaseProductResponse
        {
            ProductId = product.Id,
            ProductName = product.Name,
            Quantity = request.Quantity,
            Cost = cost.Amount,
            Change = change.Sum(c => c.Value),
            Coins = change.Select(c => c.Value)
        };

        await _dbContext.SaveChangesAsync(cancellationToken);

        return response;
    }
}