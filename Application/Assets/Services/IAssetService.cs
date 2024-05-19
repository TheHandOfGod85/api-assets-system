using SharedKernel;

namespace Application;

public interface IAssetService
{
    Task<Result<bool>> CreateAsync(Asset asset);
    Task<Result<Asset?>> GetByIdAsync(Guid id);
    Task<Result<IEnumerable<Asset>>> GetAllAsync();
    Task<Result<bool>> UpdateAsync(Asset asset);
    Task<Result<bool>> DeleteByIdAsync(Guid id);
}
