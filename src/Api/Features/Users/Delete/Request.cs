using Api.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Users.Delete;

public class DeleteUserRequest : IRequest
{
    public long UserId { get; set; }
}

public class DeleteUserRequestHandler : AsyncRequestHandler<DeleteUserRequest>
{
    private readonly VendingMachineDbContext _dbContext;

    public DeleteUserRequestHandler(VendingMachineDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override async Task Handle(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.SingleAsync(u => u.Id == request.UserId, cancellationToken);

        _dbContext.Users.Remove(user);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}