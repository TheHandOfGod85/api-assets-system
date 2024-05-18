
namespace Application;

public class AssetRepository : IAssetRepository
{
    public Task<bool> CreateAsync(Asset asset)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Asset>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Asset?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(Asset asset)
    {
        throw new NotImplementedException();
    }
}
