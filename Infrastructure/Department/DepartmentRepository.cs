using Application;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class DepartmentRepository(AssetDbContext dbContext) : IDepartmentRepository
{
    private readonly AssetDbContext _dbContext = dbContext;

    public void CreateAsync(Department department, CancellationToken cancellationToken)
    {
        _dbContext.Departments.Add(department);
    }

    public async Task<bool> Exists(string name, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Departments.AnyAsync(d => d.Name == name, cancellationToken);
    }

    public async Task<IEnumerable<Department>> GetAllAsync(CancellationToken cancellationToken)
    {
        var departments = await _dbContext.Departments.ToListAsync(cancellationToken);
        return departments;
    }

    public async Task<Department?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var department = await _dbContext.Departments.FirstOrDefaultAsync(d => d.Name == name, cancellationToken);
        return department;
    }

    public async Task<bool> IsDepartmentUnique(string name, CancellationToken cancellationToken = default)
    {
        return !await _dbContext.Departments.AnyAsync(d => d.Name == name, cancellationToken);
    }
}
