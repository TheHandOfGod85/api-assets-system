using Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IAssetRepository, AssetRepository>();
        services.AddScoped<IAppUserRepository, AppUserRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        return services;
    }

    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IEmailSender, EmailSender>();
        services.AddScoped<IUnitOfWork, AssetUnitOfWork>();
        services.AddDbContext<AssetDbContext>(options =>
        options.UseSqlServer(configuration["Database:ConnectionString"]!));
        return services;
    }
}
