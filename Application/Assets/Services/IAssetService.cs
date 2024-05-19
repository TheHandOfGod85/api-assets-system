namespace Application;

public interface IAssetService
{
    Task<bool> CreateAsync(Asset asset);
    Task<Asset?> GetByIdAsync(Guid id);
    Task<IEnumerable<Asset>> GetAllAsync();
    Task<bool> UpdateAsync(Asset asset);
    Task<bool> DeleteByIdAsync(Guid id);
}
