using System.ComponentModel.DataAnnotations;
using Domain;
using MediatR;
using SharedKernel;

namespace Application;

public class ChangeDepartmentName : IRequest<Result<bool>>
{
    public string? Name { get; set; }
    [Required(ErrorMessage = "Department name is required")]
    [MaxLength(50, ErrorMessage = "Max 50 characters for department name")]
    public string NewName { get; set; } = default!;
}

public class ChangeDepartmentNameHandler : IRequestHandler<ChangeDepartmentName, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public ChangeDepartmentNameHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(ChangeDepartmentName request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _unitOfWork.Departments.ChangeDepartmentNameAsync(request.Name, request.NewName);
            return result
            ? Result.Success(result)
            : Result.Failure<bool>(DepartmentsErrors.NotFound(request.Name));


        }
        catch (DepartmentIsUniqueException)
        {
            return Result.Failure<bool>(DepartmentsErrors.DepartmentNotUnique);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
