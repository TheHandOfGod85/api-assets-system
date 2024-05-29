using Domain;
using SharedKernel;

namespace Application;

public interface IAssetRepository
{
    Task<AssetResponse> CreateAnAssetAsync(Asset asset);
    Task<AssetResponse?> GetTheAssetByIdAsync(Guid id);
    Task<IEnumerable<AssetResponse>> GetAllTheAssetsAsync();
    Task<AssetBasicInfo?> UpdateTheAssetBasicInfoByIdAsync(Guid id, string name, string? description);
    Task<bool> CheckIfSerialNumberIsUniqueAsync(string serialNumber);
    Task<bool> DeleteAssetByIdAsync(Guid id);
    Task<bool> CheckIfTheAssetExistsByIdAsync(Guid id);
    Task<ChangeSerialNumberInfo?> ChangeSerialNumberAsync(Guid id, string serialNumber);
    Task<UpsertDepartmentInfo?> UpsertDepartmentAsync(Guid id, string departmentName);
}
