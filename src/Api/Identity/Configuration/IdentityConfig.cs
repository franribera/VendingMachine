using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Api.Identity.Configuration
{
    public static class IdentityConfig
    {
        public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

        public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
        {
            new ApiScope("vending", "Vending API")
        };

        public static IEnumerable<Client> Clients => new List<Client>
        {
            new Client
            {
                ClientId = "postman",
                ClientSecrets = {new Secret("secret".Sha256())},
                RequirePkce = true,
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "vending"
                }
            }
        };
    }
}