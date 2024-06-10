using System.Net.Http.Headers;
using Common;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Web.Api.IntegrationTests;

namespace Application.IntegrationTests;

public class UploadPhotoTest : BaseIntegrationTest
{
    private readonly IFormFile _fileMock;

    public UploadPhotoTest(IntegrationTestWebAppFactory factory) : base(factory)
    {
        _fileMock = Substitute.For<IFormFile>();
    }
    [Fact]
    public void Command_Should_Return_ValidationError_PhotoEmpty()
    {
        // Arrange
        var model = new UploadPhoto
        {
        };
        // Act
        var result = ValidatorHelper.ValidateModel(model);
        // Assert
        result.Should().ContainSingle(x => x.ErrorMessage == "The Photo field is required.");
    }
    [Fact]
    public void Command_Should_Return_ValidationError_NotAllowedFile()
    {
        // Arrange
        var fileName = "test.pdf";
        var ms = new MemoryStream();
        var writer = new StreamWriter(ms);
        writer.Flush();
        ms.Position = 0;
        _fileMock.OpenReadStream().Returns(ms);
        _fileMock.FileName.Returns(fileName);
        _fileMock.Length.Returns(ms.Length);
        var model = new UploadPhoto { Photo = _fileMock };
        // act
        var result = ValidatorHelper.ValidateModel(model);
        // assert
        result.Should().ContainSingle(x => x.ErrorMessage == "Only JPEG and PNG files are allowed.");
    }
    [Fact]
    public void Command_Should_Return_ValidationError_PhotoTooBig()
    {
        // Arrange
        var fileName = "test.png";
        var ms = new MemoryStream(new byte[2 * 1024 * 1024 + 1]);
        var writer = new StreamWriter(ms);
        writer.Flush();
        ms.Position = 0;
        _fileMock.OpenReadStream().Returns(ms);
        _fileMock.FileName.Returns(fileName);
        _fileMock.Length.Returns(ms.Length);
        var model = new UploadPhoto { Photo = _fileMock };
        // act
        var result = ValidatorHelper.ValidateModel(model);
        // assert
        result.Should().ContainSingle(x => x.ErrorMessage == "File size should not exceed 2 MB.");
    }
    [Fact]
    public async Task Handler_Should_Return_Success()
    {
        // Arrange
        var content = new MultipartFormDataContent();
        var fileContent = new StreamContent(new MemoryStream());
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
        content.Add(fileContent, "Photo", "test.png");
        await Authenticate();
        // act
        var result = await Client.PostAsync("/api/accounts/UploadPhoto", content);
        // assert
        result.StatusCode.Should().Be((System.Net.HttpStatusCode)StatusCodes.Status200OK);
    }
}
