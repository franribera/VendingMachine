using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Api.Identity.Configuration
{
    public static class IdentityConfig
    {
        public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

        public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
        {
            new(IdentityServerConstants.LocalApi.ScopeName)
        };

        public static IEnumerable<Client> Clients => new List<Client>
        {
            new()
            {
                ClientId = "postman",
                ClientSecrets = {new Secret("secret".Sha256())},
                RequirePkce = true,
                AccessTokenType = AccessTokenType.Reference,
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.LocalApi.ScopeName
                }
            },
            new()
            {
                ClientId = "swagger",
                ClientSecrets = {new Secret("secret".Sha256())},
                RequirePkce = true,
                AccessTokenType = AccessTokenType.Reference,
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.LocalApi.ScopeName
                }
            }
        };
    }
}