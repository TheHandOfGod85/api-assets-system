namespace Application;

public interface IAssetRepository
{
    Task<bool> CreateAsync(Asset asset);
    Task<Asset?> GetByIdAsync(Guid id);
    Task<IEnumerable<Asset>> GetAllAsync();
    Task<bool> UpdateAsync(Asset asset);
    Task<bool> DeleteByIdAsync(Guid id);
    Task<bool> IsSerialNumberUnique(string serialNumber);
    Task<bool> ExistsByIdAsync(Guid id);
}
