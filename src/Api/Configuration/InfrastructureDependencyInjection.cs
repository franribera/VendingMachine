using Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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
}