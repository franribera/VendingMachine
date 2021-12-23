using Api.Infrastructure.Behaviors;
using Api.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Api.Configuration;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddDbContext<VendingMachineDbContext>(options => options
            .UseSqlServer(configuration.GetConnectionString("VendingDatabase"), o =>
            {
                o.MigrationsAssembly(typeof(VendingMachineDbContext).Assembly.FullName);
            }));
    }

    public static IServiceCollection AddMediatr(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidation<,>));

        return services;
    }
}