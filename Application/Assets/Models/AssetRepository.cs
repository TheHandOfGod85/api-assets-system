using Microsoft.EntityFrameworkCore;

namespace Application;

public class AssetRepository(AssetDbContext _dbContext) : IAssetRepository
{
    public async Task<bool> CreateAsync(Asset asset)
    {
        _dbContext.Assets.Add(asset);
        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        var asset = await _dbContext.Assets.FirstOrDefaultAsync(x => x.Id == id);

        if (asset is null)
        {
            return false;
        }

        _dbContext.Remove(asset);
        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }

    public async Task<IEnumerable<Asset>> GetAllAsync()
    {
        var assets = await _dbContext.Assets.ToListAsync();
        return assets;
    }

    public async Task<Asset?> GetByIdAsync(Guid id)
    {
        var asset = await _dbContext.Assets.FirstOrDefaultAsync(x => x.Id == id);
        return asset;
    }

    public async Task<bool> UpdateAsync(Asset asset)
    {
        var assetToUpdate = await _dbContext.Assets.FirstOrDefaultAsync(x => x.Id == asset.Id);
        if (asset is null)
        {
            return false;
        }
        _dbContext.Update(asset);
        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }
}
