using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Application;

public class AssetService(
    IAssetRepository assetRepository,
    IValidator<Asset> validator,
    IUnityOfWork unityOfWork,
    ILogger<AssetService> logger) : IAssetService
{
    private readonly IAssetRepository _assetRepository = assetRepository;
    private readonly IValidator<Asset> _validator = validator;
    private readonly IUnityOfWork _unityOfWork = unityOfWork;
    private readonly ILogger<AssetService> _logger = logger;

    public async Task<bool> CreateAsync(Asset asset)
    {
        await _validator.ValidateAndThrowAsync(asset);
        _assetRepository.CreateAsync(asset);
        var result = await _unityOfWork.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        var asset = await _assetRepository.GetByIdAsync(id);
        if (asset is null) return false;
        _assetRepository.DeleteAsync(asset);
        var result = await _unityOfWork.SaveChangesAsync();
        return result > 0;
    }

    public Task<IEnumerable<Asset>> GetAllAsync()
    {
        return _assetRepository.GetAllAsync();
    }

    public Task<Asset?> GetByIdAsync(Guid id)
    {
        return _assetRepository.GetByIdAsync(id);
    }

    public async Task<bool> UpdateAsync(Asset asset)
    {

        var assetToUpdate = await _assetRepository.GetByIdAsync(asset.Id);
        if (assetToUpdate is null)
        {
            return false;
        }
        assetToUpdate.Name = asset.Name;
        assetToUpdate.Description = asset.Description;
        assetToUpdate.SerialNumber = asset.SerialNumber;
        assetToUpdate.Department = asset.Department;

        _assetRepository.UpdateAsync(assetToUpdate);

        var result = await _unityOfWork.SaveChangesAsync();

        return result > 0;
    }
}
