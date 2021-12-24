using Api.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.Users.Read;

public class ReadUserRequest : IRequest<ReadUserResponse>
{
    public long UserId { get; set; }
}

public class ReadUserResponse
{
    public long Id { get; }
    public string Username { get; }
    public string Role { get; }

    public ReadUserResponse(long id, string username, string role)
    {
        Id = id;
        Username = username;
        Role = role;
    }
}

public class ReadUserRequestHandler : IRequestHandler<ReadUserRequest, ReadUserResponse>
{
    private readonly VendingMachineDbContext _dbContext;

    public ReadUserRequestHandler(VendingMachineDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ReadUserResponse> Handle(ReadUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.SingleAsync(u => u.Id == request.UserId, cancellationToken);

        return new ReadUserResponse(user.Id, user.Username, user.Role.Name);
    }
}