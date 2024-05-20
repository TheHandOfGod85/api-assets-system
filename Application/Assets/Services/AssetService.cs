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

    public async Task<Result<bool>> CreateAsync(Asset asset)
    {
        _assetRepository.CreateAsync(asset);
        if (!await _assetRepository.IsSerialNumberUnique(asset.SerialNumber)) return Result.Failure<bool>(AssetErrors.SerialNumberNotUnique);
        var result = await _unityOfWork.SaveChangesAsync();
        return Result.Success(result > 0);
    }

    public async Task<Result<bool>> DeleteByIdAsync(Guid id)
    {
        var asset = await _assetRepository.GetByIdAsync(id);
        if (asset is null) return Result.Failure<bool>(AssetErrors.NotFound(id));
        _assetRepository.DeleteAsync(asset);
        var result = await _unityOfWork.SaveChangesAsync();
        return Result.Success(result > 0);
    }

    public async Task<Result<IEnumerable<Asset>>> GetAllAsync()
    {
        var assets = await _assetRepository.GetAllAsync();

        return Result.Success(assets);
    }

    public async Task<Result<Asset?>> GetByIdAsync(Guid id)
    {
        var asset = await _assetRepository.GetByIdAsync(id);
        if (asset is null) return Result.Failure<Asset?>(AssetErrors.NotFound(id));
        return Result.Success<Asset?>(asset);
    }

    public async Task<Result<Asset>> UpdateAsync(Asset asset)
    {

        var assetToUpdate = await _assetRepository.GetByIdAsync(asset.Id);
        if (assetToUpdate is null)
        {
            return Result.Failure<Asset>(AssetErrors.NotFound(asset.Id));
        }
        assetToUpdate.Name = asset.Name;
        assetToUpdate.Description = asset.Description;
        assetToUpdate.Department = asset.Department;

        if (assetToUpdate.SerialNumber != asset.SerialNumber)
        {
            if (!await _assetRepository.IsSerialNumberUnique(asset.SerialNumber)) return Result.Failure<Asset>(AssetErrors.SerialNumberNotUnique);
        }

        assetToUpdate.SerialNumber = asset.SerialNumber;
        _assetRepository.UpdateAsync(assetToUpdate);

        await _unityOfWork.SaveChangesAsync();

        return Result.Success(assetToUpdate);
    }
}
