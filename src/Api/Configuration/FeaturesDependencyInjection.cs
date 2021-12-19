using System.Reflection;
using MediatR;

namespace Api.Configuration;

public static class FeaturesDependencyInjection
{
    public static IServiceCollection AddFeatures(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());

        return services;
    }
}