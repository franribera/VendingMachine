using Api.Infrastructure.Persistence;
using Duende.IdentityServer;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Api.Features.Users.Read;

[Route("user")]
[Authorize(IdentityServerConstants.LocalApi.PolicyName)]
[ApiController]
public class ReadUserController : ControllerBase
{
    private readonly VendingMachineDbContext _dbContext;

    public ReadUserController(VendingMachineDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<UserDto>> Get(CancellationToken cancellationToken)
    {
        var userId = Convert.ToInt64(User.FindFirstValue(JwtClaimTypes.Id));

        var user = await _dbContext.Users.SingleAsync(u => u.Id == userId, cancellationToken);

        var response = new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Role = user.Role.Name
        };

        return Ok(response);
    }
}

public class UserDto
{
    public long Id { get; set; }
    public string Username { get; set; }
    public string Role { get; set; }
}