using Domain;
using Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using Testcontainers.PostgreSql;

namespace Web.Api.IntegrationTests;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        // .WithDatabase("api-asset-system")
        // .WithUsername("postgres")
        // .WithPassword("postgres")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services
                .SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<AssetDbContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<AssetDbContext>(options =>
            {
                options
                    .UseSqlServer(_dbContainer.GetConnectionString());
                    // .UseSnakeCaseNamingConvention();
            });

            services.AddIdentityCore<AppUser>(options =>
               {
                   options.Lockout.AllowedForNewUsers = false;
                   options.Password.RequireDigit = false;
                   options.Password.RequiredLength = 5;
                   options.Password.RequireLowercase = false;
                   options.Password.RequireUppercase = false;
                   options.Password.RequireNonAlphanumeric = false;
                   options.ClaimsIdentity.UserIdClaimType = "AppUserId";
               })
               .AddRoles<IdentityRole>()
               .AddSignInManager<SignInManager<AppUser>>()
               .AddEntityFrameworkStores<AssetDbContext>();
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AssetDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    public new Task DisposeAsync()
    {
        return _dbContainer.StopAsync();
    }
}
