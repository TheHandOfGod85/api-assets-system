using Domain;
using FluentAssertions;
using NSubstitute;

namespace Application.UnitTest;

public class DeleteAnAssetTest
{
    private readonly IUnitOfWork unitOfWork;
    private readonly DeleteAnAssetHandler _handler;

    public DeleteAnAssetTest()
    {
        unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new DeleteAnAssetHandler(unitOfWork);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenAssetIsDeleted()
    {
        // Arrange
        var assetId = Guid.NewGuid();
        var deleteAnAssetRequest = new DeleteAnAsset { Id = assetId };
        unitOfWork.Assets.DeleteAssetByIdAsync(assetId).Returns(true);

        // Act
        var result = await _handler.Handle(deleteAnAssetRequest, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        await unitOfWork.Assets.Received(1).DeleteAssetByIdAsync(assetId);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenAssetIsNotFound()
    {
        // Arrange
        var assetId = Guid.NewGuid();
        var deleteAnAssetRequest = new DeleteAnAsset { Id = assetId };
        unitOfWork.Assets.DeleteAssetByIdAsync(assetId).Returns(false);

        // Act
        var result = await _handler.Handle(deleteAnAssetRequest, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().BeEquivalentTo(AssetErrors.NotFound(assetId));
        await unitOfWork.Assets.Received(1).DeleteAssetByIdAsync(assetId);
    }
}
