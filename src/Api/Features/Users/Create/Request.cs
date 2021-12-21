using Api.Domain.Entities;
using Api.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Users.Create;

public class CreateUserRequest : IRequest<CreateUserResponse>
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}

public class CreateUserResponse
{
    public long Id { get; }
    public string Username { get; }
    public string Role { get; }

    public CreateUserResponse(long id, string username, string role)
    {
        Id = id;
        Username = username;
        Role = role;
    }
}

public class CreateUserRequestHandler : IRequestHandler<CreateUserRequest, CreateUserResponse>
{
    private readonly VendingMachineDbContext _dbContext;

    public CreateUserRequestHandler(VendingMachineDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var existingUser = await _dbContext.Users.SingleOrDefaultAsync(u => u.Username.Value == request.Username, cancellationToken);

        if (existingUser == null)
        {
            var user = new User(request.Username, request.Password, request.Role);

            await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new CreateUserResponse (user.Id, user.Username.Value, user.Role.Name);
        }

        throw new InvalidOperationException($"User {request.Username} already exists.");
    }
}