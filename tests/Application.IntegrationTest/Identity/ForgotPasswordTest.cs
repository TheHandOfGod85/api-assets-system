using Domain;
using FluentAssertions;
using Web.Api.IntegrationTests;

namespace Application.IntegrationTests;

public class ForgotPasswordTest : BaseIntegrationTest
{
    public ForgotPasswordTest(IntegrationTestWebAppFactory factory)
       : base(factory)
    {

    }
    [Fact]
    public async Task Handler_Should_Return_NotFound()
    {
        //arrange
        var command = new ForgotPassword { EmailAddress = "dan@test.com" };
        //act
        var result = await Sender.Send(command);
        //assert
        result.Error.Should().Be(AppUserErrors.UserNotFound(command.EmailAddress));
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
        var command = new ForgotPassword { EmailAddress = "dan@test.com" };

        //act
        var result = await Sender.Send(command);
        //assert
        result.IsSuccess.Should().BeTrue();
    }

}
