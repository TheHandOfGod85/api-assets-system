using Application;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class DepartmentRepository(AssetDbContext dbContext) : IDepartmentRepository
{
    private readonly AssetDbContext _dbContext = dbContext;

    public async Task<DepartmentResponse?> CreateADepartmentAsync(Department department)
    {
        _dbContext.Departments.Add(department);
        var result = await _dbContext.SaveChangesAsync();

        return result > 0
        ? new DepartmentResponse
        {
            Name = department.Name,
        }
        : null;
    }

    public async Task<bool> CheckIfADepartmentExistsAsync(string name)
    {
        return await _dbContext.Departments.AnyAsync(d => d.Name == name);
    }

    public async Task<IEnumerable<DepartmentResponse>> GetAllDepartmentsAsync()
    {
        return await _dbContext.Departments.Include(dpt => dpt.Assets).Select(department => new DepartmentResponse
        {
            Name = department.Name,
            AssetNames = department.Assets.Select(asset => asset.Name).ToList(),

        }).ToListAsync();
    }

    public async Task<Department?> GetDepartmentByNameAsync(string name)
    {
        return await _dbContext.Departments.FirstOrDefaultAsync(d => d.Name == name);
    }

    public async Task<bool> CheckIfIsDepartmentIsUniqueAsync(string name)
    {
        return !await _dbContext.Departments.AnyAsync(d => d.Name == name);
    }

    public async Task<bool> DeleteADepartmentByNameAsync(string name)
    {
        var department = await _dbContext.Departments
            .Include(dpt => dpt.Assets)
            .FirstOrDefaultAsync(dpt => dpt.Name == name);
        if (department is null) return false;
        var assets = department.Assets?.ToList();
        if (assets.Count() > 0) throw new CannotDeleteDepartmentException("The department cannot be deleted as it is assigned to assets, consider changing name instead");
        _dbContext.Remove(department);
        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> ChangeDepartmentNameAsync(string? name, string newName)
    {
        var department = await _dbContext.Departments
                 .Include(dpt => dpt.Assets)
                 .FirstOrDefaultAsync(dpt => dpt.Name == name);
        if (department is null) return false;
        if (department.Name != newName)
        {
            var isUnique = !await _dbContext.Departments.AnyAsync(d => d.Name == newName);
            if (!isUnique) throw new DepartmentIsUniqueException("Department name must be unique");
        }
        var assets = department.Assets?.ToList();
        _dbContext.Departments.Remove(department);

        var newDepartmentToChange = new Department(newName);

        foreach (var asset in assets)
        {
            asset.UpsertDepartment(newDepartmentToChange);
        }

        _dbContext.Departments.Add(newDepartmentToChange);
        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }
}
