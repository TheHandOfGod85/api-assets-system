using Application;
using Contracts;

namespace Web.API;

public static class ContractMapping
{
    public static Asset MapToAsset(this CreateAssetRequest request)
    {
        return new Asset
        {
            Name = request.Name,
            SerialNumber = request.SerialNumber,
            Department = request.Department,
            Description = request.Description,
            Id = Guid.NewGuid(),
        };
    }

    public static AssetResponse MapToAssetResponse(this Asset asset)
    {
        return new AssetResponse
        {
            Id = asset.Id,
            Name = asset.Name,
            SerialNUmber = asset.SerialNumber,
            Department = asset.Department,
            Description = asset.Description,
            CreatedAt = asset.CreatedAt,
            UpdatedAt = asset.UpdatedAt,
        };
    }

    public static AssetsResponse MapToAssetsResponse(this IEnumerable<Asset> assets)
    {
        return new AssetsResponse
        {
            Assets = assets.Select(MapToAssetResponse)
        };
    }

    public static Asset MapToAsset(this UpdateAssetRequest request, Guid id)
    {
        return new Asset
        {
            Id = id,
            Name = request.Name,
            SerialNumber = request.SerialNumber,
            Department = request.Department,
            Description = request.Description,
        };
    }
}
