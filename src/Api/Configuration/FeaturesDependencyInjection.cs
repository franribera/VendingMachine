using FluentValidation;
using System.Reflection;

namespace Api.Configuration;

public static class FeaturesDependencyInjection
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }
}