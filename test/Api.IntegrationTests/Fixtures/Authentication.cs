using Api.Domain.Entities;
using Api.Domain.Enumerations;
using Duende.IdentityServer;
using IdentityModel.Client;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Duende.IdentityServer.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.IntegrationTests.Fixtures;

public static class Authentication
{
    public static async Task AuthenticateUser(this HttpClient client, string username, string password, CancellationToken cancellationToken = default)
    {
        var discovery = await client.GetDiscoveryDocumentAsync(ConfigurationProvider.ApiBaseAddress, cancellationToken);

        var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
        {
            GrantType = GrantType.ResourceOwnerPassword,
            Address = discovery.TokenEndpoint,
            ClientId = "postman",
            ClientSecret = "secret",
            UserName = username,
            Password = password,
            Scope = IdentityServerConstants.LocalApi.ScopeName
        }, cancellationToken);

        client.SetBearerToken(tokenResponse.AccessToken);
    }

    public static async Task<User> AuthenticateDefaultUser(this HttpClient client, CancellationToken cancellationToken = default)
    {
        const string username = "Admin";
        const string password = "Admin123";

        User user;
        
        await using (var writeContext = VendingMachineDbContextFactory.Create())
        {
            user = await writeContext.Users.SingleOrDefaultAsync(u => u.Username.Value == username, cancellationToken);

            if (user == null)
            {
                user = new User(username, password, Role.Seller.Name);
                await writeContext.Users.AddAsync(user, cancellationToken);
                await writeContext.SaveChangesAsync(cancellationToken);
            }
        }

        await AuthenticateUser(client, username, password, cancellationToken);

        return user;
    }
}