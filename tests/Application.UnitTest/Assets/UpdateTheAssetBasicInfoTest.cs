using Common;
using Domain;
using FluentAssertions;
using NSubstitute;
using SharedKernel;

namespace Application.UnitTest;

public class UpdateTheAssetBasicInfoTest
{
    private readonly UpdateTheAssetBasicInfoHandler _handler;
    private readonly IUnitOfWork unitOfWorkMock;
    public UpdateTheAssetBasicInfoTest()
    {
        unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _handler = new UpdateTheAssetBasicInfoHandler(unitOfWorkMock);
    }

    [Fact]
    public void Command_Should_Have_Error_When_Name_Is_Empty()
    {
        var model = new UpdateTheAssetBasicInfo { Name = string.Empty };
        var result = ValidatorHelper.ValidateModel(model);
        result.Should().ContainSingle(x => x.ErrorMessage == "Name is required");
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WhenAssetIsUpdated()
    {
        // Arrange
        var request = new UpdateTheAssetBasicInfo
        {
            Id = Guid.NewGuid(),
            Name = "Updated Asset Name",
            Description = "Updated Asset Description"
        };

        var assetInfo = new AssetBasicInfo(request.Name, request.Description);

        unitOfWorkMock.Assets.UpdateTheAssetBasicInfoByIdAsync(request.Id, request.Name, request.Description)
            .Returns(Task.FromResult<AssetBasicInfo?>(assetInfo));

        // Act
        var result = await _handler.Handle(request, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(assetInfo);
        await unitOfWorkMock.Assets.Received(1).UpdateTheAssetBasicInfoByIdAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string?>());
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenAssetIsNotFound()
    {
        // Arrange
        var request = new UpdateTheAssetBasicInfo
        {
            Id = Guid.NewGuid(),
            Name = "Non-existent Asset",
            Description = "Non-existent Description"
        };

        unitOfWorkMock.Assets.UpdateTheAssetBasicInfoByIdAsync(request.Id, request.Name, request.Description)
            .Returns(Task.FromResult<AssetBasicInfo?>(null));

        // Act
        var result = await _handler.Handle(request, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(AssetErrors.NotFound(request.Id));
        await unitOfWorkMock.Assets.Received(1).UpdateTheAssetBasicInfoByIdAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string?>());
    }

}
