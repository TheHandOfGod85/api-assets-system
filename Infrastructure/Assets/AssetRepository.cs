using Application;
using Domain;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure;

public class AssetRepository(AssetDbContext _dbContext) : IAssetRepository
{
    public async Task<AssetResponse> CreateAnAssetAsync(Asset asset)
    {
        _dbContext.Assets.Add(asset);
        await _dbContext.SaveChangesAsync();
        return new AssetResponse
        {
            Id = asset.Id,
            Name = asset.Name,
            SerialNumber = asset.SerialNumber,
            DepartmentName = asset.Department?.Name,
            Description = asset.Description,
        };
    }

    public async Task<bool> DeleteAssetByIdAsync(Guid id)
    {
        var asset = await _dbContext.Assets.Where(asset => asset.Id == id).FirstOrDefaultAsync();
        if (asset is null) return false;
        _dbContext.Assets.Remove(asset);
        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> CheckIfTheAssetExistsByIdAsync(Guid id)
    {
        return await _dbContext.Assets.AnyAsync(x => x.Id == id);
    }


    public async Task<IEnumerable<AssetResponse>> GetAllTheAssetsAsync()
    {
        return await _dbContext.Assets
           .Select(asset => new AssetResponse
           {
               Id = asset.Id,
               Name = asset.Name,
               SerialNumber = asset.SerialNumber,
               DepartmentName = asset.Department != null ? asset.Department.Name : null,
               Description = asset.Description,
           }).ToListAsync();
    }

    public async Task<AssetResponse?> GetTheAssetByIdAsync(Guid id)
    {
        return await _dbContext.Assets
       .Where(asset => asset.Id == id)
       .Select(asset => new AssetResponse
       {
           Id = asset.Id,
           Name = asset.Name,
           SerialNumber = asset.SerialNumber,
           DepartmentName = asset.Department != null ? asset.Department.Name : null,
           Description = asset.Description,
       }).FirstOrDefaultAsync();
    }


    public async Task<AssetBasicInfo?> UpdateTheAssetBasicInfoByIdAsync(
        Guid id,
        string name,
        string? description)
    {
        var asset = await _dbContext.Assets.Where(asset => asset.Id == id).FirstOrDefaultAsync();
        if (asset == null) return null;
        asset.UpdateAsset
        (
            name,
            description
        );
        _dbContext.Assets.Update(asset);
        await _dbContext.SaveChangesAsync();
        return new AssetBasicInfo
        (
            asset.Name,
            asset.Description
        );
    }

    public async Task<bool> CheckIfSerialNumberIsUniqueAsync(string serialNumber)
    {
        return !await _dbContext.Assets.AnyAsync(x => x.SerialNumber == serialNumber);
    }

    public async Task<ChangeSerialNumberInfo?> ChangeSerialNumberAsync(Guid id, string serialNumber)
    {
        var asset = await _dbContext.Assets.Where(asset => asset.Id == id).FirstOrDefaultAsync();
        if (asset == null) return null;
        if (serialNumber != asset.SerialNumber)
        {
            var result = !await _dbContext.Assets.AnyAsync(x => x.SerialNumber == serialNumber);
            if (!result) throw new SerialNumberIsUniqueExceptions("Serial number must be unique");
        }
        asset.ChangeSerialNumber
        (
            serialNumber
        );
        _dbContext.Assets.Update(asset);
        await _dbContext.SaveChangesAsync();
        return new ChangeSerialNumberInfo
        (
            serialNumber
        );
    }

    public async Task<UpsertDepartmentInfo?> UpsertDepartmentAsync(Guid id, string departmentName)
    {
        var asset = await _dbContext.Assets.Where(asset => asset.Id == id).FirstOrDefaultAsync();
        if (asset is null) return null;
        var department = await _dbContext.Departments.Where(dpt => dpt.Name == departmentName).FirstOrDefaultAsync();
        if (department is null) throw new DepartmentNotFoundException($"Department {departmentName} not found!");
        asset.UpsertDepartment(department);
        _dbContext.Assets.Update(asset);
        await _dbContext.SaveChangesAsync();
        return new UpsertDepartmentInfo
        (
            departmentName
        );
    }
}
