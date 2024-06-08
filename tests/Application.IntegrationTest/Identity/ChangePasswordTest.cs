using Domain;
using FluentAssertions;
using Web.Api.IntegrationTests;

namespace Application.IntegrationTests;

public class ChangePasswordTest : BaseIntegrationTest
{
    public ChangePasswordTest(IntegrationTestWebAppFactory factory) : base(factory)
    {

    }
    [Fact]
    public async Task Handler_Should_Return_Error_NotFound()
    {
        //arrange
        var command = new ChangePassword
        {
            EmailAddress = "dan@test.com",
            NewPassword = "Pass673hhf",
            Token = ""
        };
        //act
        var result = await Sender.Send(command);
        //assert
        result.Error.Should().Be(AppUserErrors.UserNotFound(command.EmailAddress));
    }
    [Fact]
    public async Task Handler_Should_Return_Error_NewPasswordIsNotSameAsOld()
    {
        //arrange
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
        var forgotPassword = new ForgotPassword { EmailAddress = "dan@test.com" };
        var token = await Sender.Send(forgotPassword);
        var command = new ChangePassword
        {
            EmailAddress = "dan@test.com",
            NewPassword = "Pa$$0rd!",
            Token = token.Value
        };
        //act
        var result = await Sender.Send(command);
        //assert
        result.Error.Should().Be(AppUserErrors.NewPasswordIsNotSameAsOld);
    }
    [Fact]
    public async Task Handler_Should_Return_Error_InvalidToken()
    {
        //arrange
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
        var forgotPassword = new ForgotPassword { EmailAddress = "dan@test.com" };
        var token = await Sender.Send(forgotPassword);
        var command = new ChangePassword
        {
            EmailAddress = "dan@test.com",
            NewPassword = "Pa$$0rd!",
            Token = token.Value + "dsadasdas"
        };
        //act
        var result = await Sender.Send(command);
        //assert
        result.Error.Should().Be(AppUserErrors.NewPasswordIsNotSameAsOld);
    }
    [Fact]
    public async Task Handler_Should_Return_Success()
    {
        //arrange
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
        var forgotPassword = new ForgotPassword { EmailAddress = "dan@test.com" };
        var token = await Sender.Send(forgotPassword);
        var command = new ChangePassword
        {
            EmailAddress = "dan@test.com",
            NewPassword = "Pa$$0rd!2",
            Token = token.Value
        };
        //act
        var result = await Sender.Send(command);
        //assert
        result.IsSuccess.Should().BeTrue();
    }

}
