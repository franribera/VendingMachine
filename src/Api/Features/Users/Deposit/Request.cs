using Api.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Users.Deposit;

public class DepositRequest : IRequest<DepositResponse>
{
    public long UserId { get; set; }
    public int Coin { get; set; }
}

public class DepositResponse
{
    public long UserId { get; }
    public int Deposit { get; }

    public DepositResponse(long userId, int deposit)
    {
        UserId = userId;
        Deposit = deposit;
    }
}

public class DepositRequestHandler : IRequestHandler<DepositRequest, DepositResponse>
{
    private readonly VendingMachineDbContext _dbContext;

    public DepositRequestHandler(VendingMachineDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DepositResponse> Handle(DepositRequest request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.SingleAsync(u => u.Id == request.UserId, cancellationToken);

        user.DepositMoney(request.Coin);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new DepositResponse(user.Id, user.Deposit.Amount);
    }
}