namespace Contracts;

public class AssetsResponse
{
    public required IEnumerable<AssetResponse> Assets { get; init; } = Enumerable.Empty<AssetResponse>();
}
