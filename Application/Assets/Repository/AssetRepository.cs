using Microsoft.EntityFrameworkCore;

namespace Application;

public class AssetRepository(AssetDbContext _dbContext) : IAssetRepository
{
    public void CreateAsync(Asset asset)
    {
        _dbContext.Assets.Add(asset);
        // var result = await _dbContext.SaveChangesAsync();
        // return result > 0;
    }

    public void DeleteAsync(Asset asset)
    {
        _dbContext.Remove(asset);
    }

    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        return await _dbContext.Assets.AnyAsync(x => x.Id == id);
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

    public async Task<bool> IsSerialNumberUnique(string serialNumber)
    {
        return !await _dbContext.Assets.AnyAsync(a => a.SerialNumber == serialNumber);
    }

    public void UpdateAsync(Asset asset)
    {
        _dbContext.Update(asset);
    }
}
