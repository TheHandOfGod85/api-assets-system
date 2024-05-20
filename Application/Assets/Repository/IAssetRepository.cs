using Domain;

namespace Application;

public interface IAssetRepository
{
    void CreateAsync(Asset asset);
    Task<Asset?> GetByIdAsync(Guid id);
    Task<IEnumerable<Asset>> GetAllAsync();
    void UpdateAsync(Asset asset);
    void DeleteAsync(Asset asset);
    Task<bool> IsSerialNumberUnique(string serialNumber);
    Task<bool> ExistsByIdAsync(Guid id);
}
