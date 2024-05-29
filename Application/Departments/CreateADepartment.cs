using System.ComponentModel.DataAnnotations;
using Domain;
using MediatR;
using SharedKernel;

namespace Application;

public class CreateADepartment : IRequest<Result<DepartmentResponse?>>
{
    [Required(ErrorMessage = "Department name is required")]
    [MaxLength(50, ErrorMessage = "Max 50 characters for department name")]
    public string Name { get; set; } = default!;

}

public class CreateADepartmentHandler : IRequestHandler<CreateADepartment, Result<DepartmentResponse?>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateADepartmentHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<DepartmentResponse?>> Handle(CreateADepartment request, CancellationToken cancellationToken)
    {
        if (!await _unitOfWork.Departments.CheckIfIsDepartmentIsUniqueAsync(request.Name)) return Result.Failure<DepartmentResponse?>(DepartmentsErrors.DepartmentNotUnique);
        var department = new Department(request.Name);
        var result = await _unitOfWork.Departments.CreateADepartmentAsync(department);
        return result is not null
        ? Result.Success<DepartmentResponse?>(result)
        : Result.Failure<DepartmentResponse?>(DepartmentsErrors.DepartmentCreationError);
    }
}

