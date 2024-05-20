using Domain;

namespace Application;

public interface IAssetRepository
{
    void CreateAsync(Asset asset, CancellationToken cancellationToken = default);
    Task<Asset?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Asset>> GetAllAsync(CancellationToken cancellationToken = default);
    void UpdateAsync(Asset asset, CancellationToken cancellationToken = default);
    void DeleteAsync(Asset asset, CancellationToken cancellationToken = default);
    Task<bool> IsSerialNumberUnique(string serialNumber, CancellationToken cancellationToken = default);
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
