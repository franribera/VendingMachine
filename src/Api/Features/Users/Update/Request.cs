using Api.Domain.Entities;
using Api.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Users.Update;

public class UpdateUserRequest : IRequest<UpdateUserResponse>
{
    public long UserId { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
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
        await AssertUsernameNotInUse(request, cancellationToken);

        var user = await UpdateUser(request, cancellationToken);

        return new UpdateUserResponse(user.Id, user.Username.Value, user.Role.Name);
    }

    private async Task AssertUsernameNotInUse(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var userNameInUse = await _dbContext.Users.AnyAsync(u => u.Username.Value == request.Username && u.Id != request.UserId, cancellationToken);

        if (userNameInUse) throw new InvalidOperationException($"Username {request.Username} already in use.");
    }

    private async Task<User> UpdateUser(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null) throw new KeyNotFoundException($"User {request.UserId} does not exists.");

        user.Update(request.Username, request.Password, request.Role);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return user;
    }
}