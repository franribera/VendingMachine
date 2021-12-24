using Api.Domain.ValueObjects;
using Api.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Users.Reset;

public class ResetRequest : IRequest<ResetResponse>
{
    public long UserId { get; set; }
}

public class ResetResponse
{
    public long UserId { get; }
    public int Amount { get; }

    public IReadOnlyCollection<int> Coins { get; }

    public ResetResponse(long userId, IReadOnlyCollection<Coin> coins)
    {
        UserId = userId;
        Amount = coins.Sum(c => c.Value);
        Coins = coins.Select(c => c.Value).ToList();
    }
}

public class ResetRequestHandler : IRequestHandler<ResetRequest, ResetResponse>
{
    private readonly VendingMachineDbContext _dbContext;

    public ResetRequestHandler(VendingMachineDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ResetResponse> Handle(ResetRequest request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.SingleAsync(u => u.Id == request.UserId, cancellationToken);

        var coins = user.ResetDeposit();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ResetResponse(user.Id, coins.ToList());
    }
}