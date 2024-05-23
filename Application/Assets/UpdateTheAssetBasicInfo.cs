using System.ComponentModel.DataAnnotations;
using Domain;
using MediatR;
using SharedKernel;

namespace Application;

public class UpdateTheAssetBasicInfo : IRequest<Result<AssetBasicInfo?>>
{
    [Required(ErrorMessage = "The id is required")]
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; init; } = default!;
    [MaxLength(300, ErrorMessage = "Serial number must be max 300 characters")]
    public string? Description { get; init; }
}

public class UpdateTheAssetBasicInfoHandler : IRequestHandler<UpdateTheAssetBasicInfo, Result<AssetBasicInfo?>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTheAssetBasicInfoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AssetBasicInfo?>> Handle(UpdateTheAssetBasicInfo request, CancellationToken cancellationToken)
    {

        var result = await _unitOfWork.Assets.UpdateTheAssetBasicInfoByIdAsync(
            request.Id,
            request.Name,
            request.Description);
        if (result is null) return Result.Failure<AssetBasicInfo?>(AssetErrors.NotFound(request.Id));
        return result;
    }
}

public record AssetBasicInfo(string name, string? description);
