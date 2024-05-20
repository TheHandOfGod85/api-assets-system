using Application;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AssetRepository(AssetDbContext _dbContext) : IAssetRepository
{
    public void CreateAsync(Asset asset, CancellationToken cancellationToken)
    {
        _dbContext.Assets.Add(asset);
    }

    public void DeleteAsync(Asset asset, CancellationToken cancellationToken)
    {
        _dbContext.Remove(asset);
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Assets.AnyAsync(x => x.Id == id);
    }


    public async Task<IEnumerable<Asset>> GetAllAsync(CancellationToken cancellationToken)
    {
        var assets = await _dbContext.Assets.ToListAsync();
        return assets;
    }

    public async Task<Asset?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var asset = await _dbContext.Assets.FirstOrDefaultAsync(x => x.Id == id);
        return asset;
    }

    public async Task<bool> IsSerialNumberUnique(string serialNumber, CancellationToken cancellationToken)
    {
        return !await _dbContext.Assets.AnyAsync(a => a.SerialNumber == serialNumber);
    }

    public void UpdateAsync(Asset asset, CancellationToken cancellationToken)
    {
        _dbContext.Update(asset);
    }
}
