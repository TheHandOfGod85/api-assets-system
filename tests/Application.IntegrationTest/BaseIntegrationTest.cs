using System.Net.Http.Headers;
using Application;
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
    protected readonly HttpClient Client;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();

        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        DbContext = _scope.ServiceProvider.GetRequiredService<AssetDbContext>();
        UserManager = _scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
        RoleManager = _scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        SignInManager = _scope.ServiceProvider.GetRequiredService<SignInManager<AppUser>>();
        Client = factory.CreateClient();
    }

    protected async Task Authenticate()
    {
        var registerConfirm = new RegisterANewUserAndSendEmail
        {
            EmailAddress = "dan@test.com",
            FirstName = "dan",
            LastName = "Del Piano",
            Role = Role.AppUser
        };
        var registerConfirmResponse = await Sender.Send(registerConfirm);
        var completedRegistration = new CompleteRegistrationFromEmail
        {
            EmailAddress = "dan@test.com",
            Password = "Pa$$0rd!",
            Token = registerConfirmResponse.Value
        };
        await Sender.Send(completedRegistration);
        var loginCommand = new Login { EmailAddress = "dan@test.com", Password = "Pa$$0rd!" };
        var loginResponse = await Sender.Send(loginCommand);

        var token = loginResponse.Value;
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
