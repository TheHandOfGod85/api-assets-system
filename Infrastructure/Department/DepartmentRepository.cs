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
        return await _dbContext.Departments.Select(department => new DepartmentResponse
        {
            Name = department.Name,

        }).ToListAsync();
    }

    public async Task<DepartmentResponse?> GetDepartmentByNameAsync(string name)
    {
        return await _dbContext.Departments.Select(department => new DepartmentResponse
        {
            Name = department.Name
        }
        ).FirstOrDefaultAsync(d => d.Name == name);
    }

    public async Task<bool> CheckIfIsDepartmentIsUniqueAsync(string name)
    {
        return !await _dbContext.Departments.AnyAsync(d => d.Name == name);
    }
}
