using Contracts;
using Domain;

namespace Web.API;

public static class AssetMapping
{
    public static Asset MapToAsset(this CreateAssetRequest request)
    {
        if (request.Department is not null)
        {
            var department = new Department(request.Department);
            return new Asset
                    (
                        request.Name,
                        request.SerialNumber,
                        department,
                        request.Description,
                        Guid.NewGuid()
                    );
        }
        else
        {

            return new Asset
            (
                request.Name,
                request.SerialNumber,
                null,
                request.Description,
                Guid.NewGuid()
            );
        }
    }

    public static AssetResponse MapToAssetResponse(this Asset asset)
    {
        return new AssetResponse
        {
            Id = asset.Id,
            Name = asset.Name,
            SerialNumber = asset.SerialNumber,
            Department = asset.Department?.MapToDepartmentResponse(),
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
        var department = new Department(request.Department);
        return new Asset
        (
            request.Name,
            request.SerialNumber,
            department,
            request.Description,
            id
        );
    }
}
