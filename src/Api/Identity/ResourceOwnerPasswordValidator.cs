using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using IdentityModel;

namespace Api.Identity;

public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
{
    public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        var user = TestUsers.Users.SingleOrDefault(u => u.Username == context.UserName);

        if (user == null)
        {
            context.Result = new GrantValidationResult(TokenRequestErrors.UnauthorizedClient);
        }
        else
        {
            context.Result = new GrantValidationResult(
                context.UserName,
                OidcConstants.AuthenticationMethods.Password,
                DateTime.UtcNow,
                TestUsers.Users.First().Claims);
        }

        return Task.CompletedTask;
    }
}