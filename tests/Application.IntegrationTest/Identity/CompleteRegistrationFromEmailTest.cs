using Application;
using Domain;
using FluentAssertions;
using SharedKernel;

namespace Web.Api.IntegrationTests;

public class CompleteRegistrationFromEmailTest : BaseIntegrationTest
{
    public CompleteRegistrationFromEmailTest(IntegrationTestWebAppFactory factory)
     : base(factory)
    {

    }
    [Fact]
    public async Task Handler_Should_Return_Error_Invalid_Token()
    {
        var command = new CompleteRegistrationFromEmail
        {
            EmailAddress = "dan@test.com",
            Password = "Pa$$0rd!",
            Token = string.Empty
        };
        //act
        Result<RegistrationResult> respone = await Sender.Send(command);
        //assert
        respone.Error.Should().Be(AppUserErrors.InvalidToken);
    }
    [Fact]
    public async Task Handler_Should_Return_Error_If_User_Not_Found()
    {
        //arrange
        var command = new RegisterANewUserAndSendEmail
        {
            EmailAddress = "dan@test.com",
            FirstName = "dan",
            LastName = "Del Piano",
            Role = Role.AppUser
        };
        Result<string> response = await Sender.Send(command);
        var command2 = new CompleteRegistrationFromEmail
        {
            EmailAddress = "dany@test.com",
            Password = "Pa$$0rd!",
            Token = response.Value
        };
        //act
        Result<RegistrationResult> result = await Sender.Send(command2);
        //assert
        result.Error.Should().Be(AppUserErrors.UserNotFound(command2.EmailAddress));
    }
    [Fact]
    public async Task Handler_Should_Return_Error_If_User_Already_ConfirmedEmailAddress()
    {
        //arrange
        var command = new RegisterANewUserAndSendEmail
        {
            EmailAddress = "dan@test.com",
            FirstName = "dan",
            LastName = "Del Piano",
            Role = Role.AppUser
        };
        Result<string> response = await Sender.Send(command);
        var command2 = new CompleteRegistrationFromEmail
        {
            EmailAddress = "dan@test.com",
            Password = "Pa$$0rd!",
            Token = response.Value
        };
        var command3 = new CompleteRegistrationFromEmail
        {
            EmailAddress = "dan@test.com",
            Password = "Pa$$0rd!",
            Token = response.Value
        };
        //act
        await Sender.Send(command2);
        Result<RegistrationResult> result2 = await Sender.Send(command3);
        //assert
        result2.Error.Should().Be(AppUserErrors.UserAlreadyConfirmed);
    }

    [Fact]
    public async Task Handler_Should_Return_Success_If_Completed_The_Registration()
    {
        //arrange
        var command = new RegisterANewUserAndSendEmail
        {
            EmailAddress = "dan@test.com",
            FirstName = "dan",
            LastName = "Del Piano",
            Role = Role.AppUser
        };
        Result<string> response = await Sender.Send(command);
        var command2 = new CompleteRegistrationFromEmail
        {
            EmailAddress = "dan@test.com",
            Password = "Pa$$0rd!",
            Token = response.Value
        };
        //act
        Result<RegistrationResult> result = await Sender.Send(command2);
        //assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(result.Value);
    }
}
