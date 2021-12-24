using System.Security.Claims;
using Api.Domain.Entities;
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
        var user = await GetUser(context.Subject);

        if (user != null)
        {
            context.IssuedClaims.Add(new Claim(JwtClaimTypes.Role, user.Role.Name.ToLower()));
            context.IssuedClaims.Add(new Claim(JwtClaimTypes.Id, user.Id.ToString()));
        }
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var user = await GetUser(context.Subject);

        context.IsActive = user != null;
    }

    private async Task<User?> GetUser(ClaimsPrincipal subject)
    {
        var username = subject.FindFirstValue(JwtClaimTypes.Subject);

        return await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == username);
    }
}