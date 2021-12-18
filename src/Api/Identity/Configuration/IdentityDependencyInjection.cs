using Duende.IdentityServer.Validation;

namespace Api.Identity.Configuration;

public static class IdentityDependencyInjection
{
    public static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddIdentityServer()
        //    .AddInMemoryIdentityResources(IdentityConfig.IdentityResources)
        //    .AddInMemoryApiScopes(IdentityConfig.ApiScopes)
        //    .AddInMemoryClients(IdentityConfig.Clients)
        //    .AddTestUsers(TestUsers.Users);

        services.AddIdentityServer()
            .AddInMemoryApiScopes(IdentityConfig.ApiScopes)
            .AddInMemoryClients(IdentityConfig.Clients);

        services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();

        return services;
    }
}