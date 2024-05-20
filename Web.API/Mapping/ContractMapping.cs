using Contracts;
using Domain;

namespace Web.API;

public static class ContractMapping
{
    public static Asset MapToAsset(this CreateAssetRequest request)
    {
        return new Asset
        (
            request.Name,
            request.Department,
            request.SerialNumber,
            request.Description,
            Guid.NewGuid()
        );
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
        (
            request.Name,
            request.Department,
            request.SerialNumber,
            request.Description,
            id
        );
    }
}
