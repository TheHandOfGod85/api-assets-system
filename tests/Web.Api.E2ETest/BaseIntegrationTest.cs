using Domain;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Api.IntegrationTests;

public class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>
{
    private readonly IServiceScope _scope;
    protected readonly ISender Sender;
    protected readonly AssetDbContext DbContext;
    protected readonly UserManager<AppUser> UserManager;
    protected readonly RoleManager<IdentityRole> RoleManager;
    protected readonly SignInManager<AppUser> SignInManager;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();

        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        DbContext = _scope.ServiceProvider.GetRequiredService<AssetDbContext>();
        UserManager = _scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        RoleManager = _scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        SignInManager = _scope.ServiceProvider.GetRequiredService<SignInManager<AppUser>>();
    }
}
