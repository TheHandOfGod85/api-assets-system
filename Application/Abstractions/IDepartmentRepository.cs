using Domain;

namespace Application;

public interface IDepartmentRepository
{
    Task<DepartmentResponse> CreateADepartmentAsync(Department department);
    Task<IEnumerable<Department>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> CheckIfIsDepartmentIsUnique(string name, CancellationToken cancellationToken = default);
    Task<Department?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<bool> Exists(string name, CancellationToken cancellationToken = default);
}
