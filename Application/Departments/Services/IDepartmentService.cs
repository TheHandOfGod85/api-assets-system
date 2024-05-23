using Domain;
using SharedKernel;

namespace Application;

public interface IDepartmentService
{
    // Task<Result<bool>> CreateADepartmentAsync(
    //     Department department,
    //     CancellationToken cancellationToken);
    Task<Result<IEnumerable<Department>>> GetAllDepartmentsAsync(CancellationToken cancellationToken);
}
