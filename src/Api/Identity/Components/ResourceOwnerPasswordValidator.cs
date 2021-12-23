using System.Security.Claims;
using Api.Domain.ValueObjects;
using Api.Infrastructure.Persistence;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using IdentityModel;
using Microsoft.EntityFrameworkCore;

namespace Api.Identity.Components;

public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
{
    private readonly VendingMachineDbContext _dbContext;

    public ResourceOwnerPasswordValidator(VendingMachineDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Username == context.UserName);

        if (user == null)
        {
            context.Result = new GrantValidationResult(TokenRequestErrors.UnauthorizedClient);
        }
        else
        {
            var password = new Password(context.Password);

            if (Equals(user.Password, password))
            {
                context.Result = new GrantValidationResult(
                    context.UserName,
                    OidcConstants.AuthenticationMethods.Password,
                    DateTime.UtcNow,
                    new List<Claim> { new(JwtClaimTypes.Role, user.Role.Name) });
            }
            else
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.UnauthorizedClient);
            }
        }
    }
}