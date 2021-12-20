using Api.Domain.Entities;
using Api.Infrastructure.Persistence;
using MediatR;

namespace Api.Features.Users.Create;

public class CreateUserRequest : IRequest<CreateUserResponse>
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
}

public class CreateUserResponse
{
    public long Id { get; set; }
    public string Username { get; set; }
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
        var user = new User(request.Username, request.Password, request.Role);

        await _dbContext.Users.AddAsync(user, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new CreateUserResponse
        {
            Id = user.Id,
            Username = user.Username.Value
        };
    }
}