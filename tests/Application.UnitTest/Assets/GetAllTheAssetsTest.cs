using FluentAssertions;
using NSubstitute;

namespace Application.UnitTest;

public class GetAllTheAssetsTest
{
    private readonly IUnitOfWork unitOfWorkMock;
    private readonly GetAllTheAssetsHandler _handler;

    public GetAllTheAssetsTest()
    {
        unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _handler = new GetAllTheAssetsHandler(unitOfWorkMock);
    }


    [Fact]
    public async Task Handle_ShouldReturnSuccessResultWithAssets_WhenAssetsExist()
    {
        // Arrange
        var assets = new List<AssetResponse>
        {
            new AssetResponse { Id = Guid.NewGuid(), Name = "Asset1",Description="Some description" },
            new AssetResponse { Id = Guid.NewGuid(), Name = "Asset2",Description="Some description 2"}
        };

        unitOfWorkMock.Assets.GetAllTheAssetsAsync().Returns(Task.FromResult<IEnumerable<AssetResponse>>(assets));

        var request = new GetAllTheAssets();

        // Act
        var result = await _handler.Handle(request, default);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(assets);
        await unitOfWorkMock.Assets.Received(1).GetAllTheAssetsAsync();

    }

}
