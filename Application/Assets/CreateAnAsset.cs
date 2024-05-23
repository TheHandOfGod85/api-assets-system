using System.ComponentModel.DataAnnotations;
using Domain;
using MediatR;
using SharedKernel;

namespace Application;
public class CreateAnAsset() : IRequest<Result<AssetResponse>>
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; init; } = default!;
    [Required(ErrorMessage = "Serial number is required")]
    [MaxLength(50, ErrorMessage = "Serial number must be max 50 characters")]
    public string SerialNumber { get; init; } = default!;
    [MaxLength(300, ErrorMessage = "Serial number must be max 300 characters")]
    public string? Description { get; init; }
    // [Required(ErrorMessage = "Department is required")]
    // [DepartmentValidator]
    [MaxLength(30, ErrorMessage = "Department must be max 50 characters")]
    public string? Department { get; init; }
}
public class CreateAnAssetHandler : IRequestHandler<CreateAnAsset, Result<AssetResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateAnAssetHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AssetResponse>> Handle(CreateAnAsset request, CancellationToken cancellationToken)
    {

        if (!await _unitOfWork.Assets.CheckIfSerialNumberIsUniqueAsync(request.SerialNumber)) return Result.Failure<AssetResponse>(AssetErrors.SerialNumberNotUnique);
        if (request.Department is null)
        {
            var newAsset = new Asset
            (
                request.Name,
                request.SerialNumber,
                null,
                request.Description
            );
            var result = await _unitOfWork.Assets.CreateAnAssetAsync(newAsset);
            return Result.Success(result);
        }
        else
        {
            if (!await _unitOfWork.Departments.Exists(request.Department, cancellationToken)) return Result.Failure<AssetResponse>(DepartmentsErrors.NotFound(request.Department));
            var department = await _unitOfWork.Departments.GetByNameAsync(request.Department);
            var newAsset = new Asset
            (
                request.Name,
                request.SerialNumber,
                department,
                request.Description
            );
            var result = await _unitOfWork.Assets.CreateAnAssetAsync(newAsset);
            return Result.Success(result);
        }
    }
}