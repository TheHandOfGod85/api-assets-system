using Domain;
using SharedKernel;

namespace Application;

public class DepartmentService(
    IUnitOfWork unityOfWork) : IDepartmentService
{
    private readonly IUnitOfWork _unityOfWork = unityOfWork;

    // public async Task<Result<bool>> CreateADepartmentAsync(Department department, CancellationToken cancellationToken)
    // {
    //     if (!await _departmentRepository.IsDepartmentUnique(department.Name, cancellationToken)) return Result.Failure<bool>(DepartmentsErrors.DepartmentNotUnique);
    //     _departmentRepository.CreateAsync(department, cancellationToken);
    //     var result = await _unityOfWork.SaveChangesAsync(cancellationToken);
    //     return Result.Success(result > 0);
    // }

    public async Task<Result<IEnumerable<Department>>> GetAllDepartmentsAsync(CancellationToken cancellationToken)
    {
        var departments = await _departmentRepository.GetAllAsync(cancellationToken);
        return Result.Success(departments);
    }
}
