using Domain;

namespace Application;

public interface IDepartmentRepository
{
    void CreateAsync(Department department, CancellationToken cancellationToken = default);
    Task<IEnumerable<Department>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> IsDepartmentUnique(string name, CancellationToken cancellationToken = default);
    Task<Department?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> Exists(string name, CancellationToken cancellationToken = default);
}
