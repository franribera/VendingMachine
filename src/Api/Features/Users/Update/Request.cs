using Api.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Users.Update;

public class UpdateUserRequest : IRequest<UpdateUserResponse>
{
    public long UserId { get; set; }
    public string Password { get; set; }
}

public class UpdateUserResponse
{
    public long Id { get; }
    public string Username { get; }
    public string Role { get; }

    public UpdateUserResponse(long id, string username, string role)
    {
        Id = id;
        Username = username;
        Role = role;
    }
}

public class UpdateUserRequestHandler : IRequestHandler<UpdateUserRequest, UpdateUserResponse>
{
    private readonly VendingMachineDbContext _dbContext;

    public UpdateUserRequestHandler(VendingMachineDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UpdateUserResponse> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.SingleAsync(u => u.Id == request.UserId, cancellationToken);

        user.UpdatePassword(request.Password);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateUserResponse(user.Id, user.Username, user.Role.Name);
    }
}