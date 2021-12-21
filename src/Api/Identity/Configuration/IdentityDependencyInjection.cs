using Api.Identity.Components;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Validation;

namespace Api.Identity.Configuration;

public static class IdentityDependencyInjection
{
    public static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddLocalApiAuthentication()
            .AddIdentityServer()
            .AddInMemoryIdentityResources(IdentityConfig.IdentityResources)
            .AddInMemoryApiScopes(IdentityConfig.ApiScopes)
            .AddInMemoryClients(IdentityConfig.Clients)
            .AddProfileService<ProfileService>();

        services.AddTransient<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();

        return services;
    }
}