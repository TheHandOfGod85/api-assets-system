using Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IAssetRepository, AssetRepository>();
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IUnityOfWork>(sp => sp.GetRequiredService<AssetDbContext>());
        services.AddDbContext<AssetDbContext>(options => options.UseSqlServer(connectionString));
        return services;
    }

}
