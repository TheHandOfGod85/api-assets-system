using Domain;
using FluentAssertions;
using Web.Api.IntegrationTests;

namespace Application.IntegrationTests;

public class LoginTest : BaseIntegrationTest
{
    public LoginTest(IntegrationTestWebAppFactory factory)
        : base(factory)
    {

    }
    [Fact]
    public async Task Login_Returns_Error_UserNotFound()
    {
        //arrange
        var command = new Login { EmailAddress = "dan@test.com", Password = "Pa$$0rd!" };
        //act
        var result = await Sender.Send(command);
        //assert
        result.Error.Should().Be(AppUserErrors.UserNotFound(command.EmailAddress));
    }
    [Fact]
    public async Task Login_Returns_Error_EmailNotConfirmed()
    {
        //arrange
        var command = new Login { EmailAddress = "dan@test.com", Password = "Pa$$0rd!" };
        var registerNotConfirm = new RegisterANewUserAndSendEmail
        {
            EmailAddress = "dan@test.com",
            FirstName = "dan",
            LastName = "Del Piano",
            Role = Role.AppUser
        };
        await Sender.Send(registerNotConfirm);
        //act
        var result = await Sender.Send(command);
        //assert
        result.Error.Should().Be(AppUserErrors.EmailNotConfirmed(command.EmailAddress));
    }
    [Fact]
    public async Task Login_Returns_Error_InvalidCredentials()
    {
        //arrange
        var registerNotConfirm = new RegisterANewUserAndSendEmail
        {
            EmailAddress = "dan@test.com",
            FirstName = "dan",
            LastName = "Del Piano",
            Role = Role.AppUser
        };
        var registerNotConfirmResponse = await Sender.Send(registerNotConfirm);
        var completedRegistration = new CompleteRegistrationFromEmail
        {
            EmailAddress = "dan@test.com",
            Password = "Pa$$0rd!",
            Token = registerNotConfirmResponse.Value
        };
        await Sender.Send(completedRegistration);
        var command = new Login { EmailAddress = "dan@test.com", Password = "wrongPassword" };
        //act
        var result = await Sender.Send(command);
        //assert
        result.Error.Should().Be(AppUserErrors.InvalidCredentials);
    }
    [Fact]
    public async Task Login_Returns_SuccesToken()
    {
        //arrange
        var registerNotConfirm = new RegisterANewUserAndSendEmail
        {
            EmailAddress = "dan@test.com",
            FirstName = "dan",
            LastName = "Del Piano",
            Role = Role.AppUser
        };
        var registerNotConfirmResponse = await Sender.Send(registerNotConfirm);
        var completedRegistration = new CompleteRegistrationFromEmail
        {
            EmailAddress = "dan@test.com",
            Password = "Pa$$0rd!",
            Token = registerNotConfirmResponse.Value
        };
        await Sender.Send(completedRegistration);
        var command = new Login { EmailAddress = "dan@test.com", Password = "Pa$$0rd!" };
        //act
        var result = await Sender.Send(command);
        //assert
        result.IsSuccess.Should().BeTrue();
    }

}
