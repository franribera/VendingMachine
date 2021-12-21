using System.Security.Claims;
using Api.Infrastructure.Persistence;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.EntityFrameworkCore;

namespace Api.Identity.Components;

public class ProfileService : IProfileService
{
    private readonly VendingMachineDbContext _dbContext;

    public ProfileService(VendingMachineDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var username = context.Subject.FindFirstValue(JwtClaimTypes.Subject);

        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Username.Value == username);

        if (user != null)
        {
            context.IssuedClaims.Add(new Claim(JwtClaimTypes.Role, user.Role.Name.ToLower()));
            context.IssuedClaims.Add(new Claim(JwtClaimTypes.Id, user.Id.ToString()));
        }
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        context.IsActive = true;

        return Task.CompletedTask;
    }
}