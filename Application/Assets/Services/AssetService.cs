using SharedKernel;
using Domain;
using Microsoft.Extensions.Logging;

namespace Application;

public class AssetService(
    IAssetRepository assetRepository,
    IUnityOfWork unityOfWork,
    ILogger<AssetService> logger) : IAssetService
{
    private readonly IAssetRepository _assetRepository = assetRepository;
    private readonly IUnityOfWork _unityOfWork = unityOfWork;
    private readonly ILogger<AssetService> _logger = logger;

    public async Task<Result<bool>> CreateAsync(
        Asset asset,
        CancellationToken cancellationToken)
    {
        _assetRepository.CreateAsync(asset);
        if (!await _assetRepository.IsSerialNumberUnique(asset.SerialNumber, cancellationToken)) return Result.Failure<bool>(AssetErrors.SerialNumberNotUnique);
        var result = await _unityOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(result > 0);
    }

    public async Task<Result<bool>> DeleteByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var asset = await _assetRepository.GetByIdAsync(id, cancellationToken);
        if (asset is null) return Result.Failure<bool>(AssetErrors.NotFound(id));
        _assetRepository.DeleteAsync(asset, cancellationToken);
        var result = await _unityOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(result > 0);
    }

    public async Task<Result<IEnumerable<Asset>>> GetAllAsync(CancellationToken cancellationToken)
    {
        var assets = await _assetRepository.GetAllAsync(cancellationToken);

        return Result.Success(assets);
    }

    public async Task<Result<Asset?>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var asset = await _assetRepository.GetByIdAsync(id, cancellationToken);
        if (asset is null) return Result.Failure<Asset?>(AssetErrors.NotFound(id));
        return Result.Success<Asset?>(asset);
    }

    public async Task<Result<Asset>> UpdateAsync(
        Asset asset,
        CancellationToken cancellationToken)
    {

        var assetToUpdate = await _assetRepository.GetByIdAsync(asset.Id, cancellationToken);
        if (assetToUpdate is null)
        {
            return Result.Failure<Asset>(AssetErrors.NotFound(asset.Id));
        }
        if (assetToUpdate.SerialNumber != asset.SerialNumber)
        {
            if (!await _assetRepository.IsSerialNumberUnique(asset.SerialNumber)) return Result.Failure<Asset>(AssetErrors.SerialNumberNotUnique);
        }

        assetToUpdate.UpdateAsset
        (
            asset.Id,
            asset.Name,
            asset.Department,
            asset.SerialNumber,
            asset.Description
        );

        _assetRepository.UpdateAsync(assetToUpdate, cancellationToken);

        await _unityOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(asset);
    }
}
