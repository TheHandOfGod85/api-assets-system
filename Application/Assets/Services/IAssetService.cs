using Domain;
using SharedKernel;

namespace Application;

public interface IAssetService
{
    Task<Result<bool>> CreateAsync(
        Asset asset,
        CancellationToken cancellationToken = default);
    Task<Result<Asset?>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
    Task<Result<IEnumerable<Asset>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Result<Asset>> UpdateAsync(
        Asset asset,
        CancellationToken cancellationToken = default);
    Task<Result<bool>> DeleteByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
