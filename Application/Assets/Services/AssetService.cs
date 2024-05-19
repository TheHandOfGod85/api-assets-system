
using System.Data.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application;

public class AssetService(
    IAssetRepository assetRepository,
    IValidator<Asset> validator,
    ILogger<AssetService> logger) : IAssetService
{
    private readonly IAssetRepository _assetRepository = assetRepository;
    private readonly IValidator<Asset> _validator = validator;
    private readonly ILogger<AssetService> _logger = logger;

    public async Task<bool> CreateAsync(Asset asset)
    {
        await _validator.ValidateAndThrowAsync(asset);
        return await _assetRepository.CreateAsync(asset);
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
        try
        {
            var assetExists = await _assetRepository.ExistsByIdAsync(asset.Id);
            if (!assetExists)
            {
                return null;
            }

            await _assetRepository.UpdateAsync(asset);

            return asset;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }
}
