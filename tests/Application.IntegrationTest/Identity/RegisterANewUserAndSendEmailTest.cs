using Application;
using Azure.Core;
using Domain;
using FluentAssertions;
using NSubstitute;

namespace Web.Api.IntegrationTests;

public class RegisterANewUserAndSendEmailTest : BaseIntegrationTest
{
    private readonly IEmailSender emailSender;
    public RegisterANewUserAndSendEmailTest(IntegrationTestWebAppFactory factory)
     : base(factory)
    {
        emailSender = Substitute.For<IEmailSender>();
    }
    [Fact]
    public async Task Handler_Should_Return_Error_WhenAUserAlreadyExists()
    {
        //arrange
        var command = new RegisterANewUserAndSendEmail
        {
            FirstName = "dan",
            LastName = "Del",
            EmailAddress = "dan@test.com",
            Role = Role.AppUser
        };
        await Sender.Send(command);
        //act
        var response = await Sender.Send(command);
        //assert
        response.Error.Should().Be(AppUserErrors.UserAlreadyExists);
    }
    [Fact]
    public async Task Handler_Should_Return_Token_WhenUserRegisteredSuccessfully()
    {
        //arrange
        var command = new RegisterANewUserAndSendEmail
        {
            FirstName = "dan",
            LastName = "Del",
            EmailAddress = "dan@test.com",
            Role = Role.AppUser
        };
        emailSender.SendEmail(command.EmailAddress, command.FirstName, "test", "registration", null);
        //act
        var response = await Sender.Send(command);
        //assert
        response.IsSuccess.Should().BeTrue();
        response.Value.Should().NotBeNullOrEmpty();
        emailSender.Received(1).SendEmail(command.EmailAddress, command.FirstName, "test", "registration", null);
    }
}
