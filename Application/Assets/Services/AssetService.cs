
namespace Application;

public class AssetService(IAssetRepository assetRepository) : IAssetService
{
    private readonly IAssetRepository _assetRepository = assetRepository;
    public Task<bool> CreateAsync(Asset asset)
    {
        return _assetRepository.CreateAsync(asset);
    }

    public Task<bool> DeleteByIdAsync(Guid id)
    {
        return _assetRepository.DeleteByIdAsync(id);
    }

    public Task<IEnumerable<Asset>> GetAllAsync()
    {
        return _assetRepository.GetAllAsync();
    }

    public Task<Asset?> GetByIdAsync(Guid id)
    {
        return _assetRepository.GetByIdAsync(id);
    }

    public async Task<Asset?> UpdateAsync(Asset asset)
    {
        var assetExists = await _assetRepository.ExistsByIdAsync(asset.Id);
        if (!assetExists)
        {
            return null;
        }

        await _assetRepository.UpdateAsync(asset);

        return asset;
    }
}
