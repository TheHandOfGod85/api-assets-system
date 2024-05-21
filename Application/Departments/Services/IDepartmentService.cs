using Domain;
using SharedKernel;

namespace Application;

public interface IDepartmentService
{
    Task<Result<bool>> CreateAsync(
        Department department,
        CancellationToken cancellationToken);
    Task<Result<IEnumerable<Department>>> GetAllAsync(CancellationToken cancellationToken);
}
