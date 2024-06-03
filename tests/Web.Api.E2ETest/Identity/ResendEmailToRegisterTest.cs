using Application;
using Domain;
using FluentAssertions;
using NSubstitute;
using SharedKernel;

namespace Web.Api.IntegrationTests;

public class ResendEmailToRegisterTest : BaseIntegrationTest
{
    private readonly IEmailSender emailSender;
    public ResendEmailToRegisterTest(IntegrationTestWebAppFactory factory)
       : base(factory)
    {
        emailSender = Substitute.For<IEmailSender>();
    }
    [Fact]
    public async Task Should_Return_Error_If_User_Is_Not_Found()
    {
        //arrange
        var command = new ResendEmailToRegister { EmailAddress = "dan@test.com" };
        //act
        Result<string> result = await Sender.Send(command);
        //assert
        result.Error.Should().Be(AppUserErrors.UserNotFound(command.EmailAddress));
    }
    [Fact]
    public async Task Should_Return_Token_If_Successful()
    {
        //arrange
        var command1 = new RegisterANewUserAndSendEmail
        {
            EmailAddress = "dan@test.com",
            FirstName = "dan",
            LastName = "Del Piano",
            Role = Role.AppUser
        };
        await Sender.Send(command1);
        var command = new ResendEmailToRegister { EmailAddress = "dan@test.com" };
        emailSender.SendEmail(command.EmailAddress, command1.FirstName, "test", "registration", null);
        //act
        Result<string> result = await Sender.Send(command);
        //assert
        result.IsSuccess.Should().BeTrue();
        emailSender.Received(1).SendEmail(
            command.EmailAddress,
            command1.FirstName,
            "test",
            "registration",
            null);

    }
}
