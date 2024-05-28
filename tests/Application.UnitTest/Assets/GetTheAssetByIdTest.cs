using Domain;
using FluentAssertions;
using NSubstitute;

namespace Application.UnitTest;

public class GetTheAssetByIdTest
{
    private readonly IUnitOfWork unitOfWorkMock;
    private readonly GetTheAssetByIdHandler _handler;

    public GetTheAssetByIdTest()
    {
        unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _handler = new GetTheAssetByIdHandler(unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResultWithAsset_WhenAssetExist()
    {
        // Arrange
        var asset = new AssetResponse
        {
            Id = Guid.NewGuid(),
            Name = "Asset1",
            Description = "Some description"
        };

        unitOfWorkMock.Assets.GetTheAssetByIdAsync(asset.Id).Returns(Task.FromResult<AssetResponse?>(asset));

        var request = new GetTheAssetById { Id = asset.Id };

        // Act
        var result = await _handler.Handle(request, default);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(asset);
        await unitOfWorkMock.Assets.Received(1).GetTheAssetByIdAsync(Arg.Any<Guid>());
    }
    [Fact]
    public async Task Handle_AssetDoesNotExist_ReturnsFailureResult()
    {
        // Arrange
        var assetId = Guid.NewGuid();
        unitOfWorkMock.Assets.GetTheAssetByIdAsync(assetId).Returns(Task.FromResult<AssetResponse?>(null));

        var request = new GetTheAssetById { Id = assetId };

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().BeEquivalentTo(AssetErrors.NotFound(assetId));
    }
}
