using System.ComponentModel.DataAnnotations;
using Domain;
using MediatR;
using SharedKernel;

namespace Application;

public class UpsertDepartment : IRequest<Result<UpsertDepartmentInfo?>>
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Department is required")]
    [MaxLength(50, ErrorMessage = "Max 50 characters for department name")]
    public string DepartmentName { get; set; } = default!;
}


public class UpsertDepartmentHandler : IRequestHandler<UpsertDepartment, Result<UpsertDepartmentInfo?>>
{
    private readonly IUnitOfWork _unitOfWork;
    public UpsertDepartmentHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UpsertDepartmentInfo?>> Handle(UpsertDepartment request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _unitOfWork.Assets.UpsertDepartmentAsync(request.Id, request.DepartmentName);
            return result is not null
            ? Result.Success<UpsertDepartmentInfo?>(result)
            : Result.Failure<UpsertDepartmentInfo?>(AssetErrors.NotFound(request.Id));

        }
        catch (DepartmentNotFoundException)
        {
            return Result.Failure<UpsertDepartmentInfo?>(DepartmentsErrors.NotFound(request.DepartmentName));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}

public record UpsertDepartmentInfo(string department);

