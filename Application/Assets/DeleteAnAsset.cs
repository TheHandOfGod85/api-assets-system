using Domain;
using MediatR;
using SharedKernel;

namespace Application;

public class DeleteAnAsset : IRequest<Result<bool>>
{
    public Guid Id { get; set; }

}

public class DeleteAnAssetHandler : IRequestHandler<DeleteAnAsset, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAnAssetHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteAnAsset request, CancellationToken cancellationToken)
    {
        var result = await _unitOfWork.Assets.DeleteAssetByIdAsync(request.Id);
        return result ? Result.Success<bool>(result) : Result.Failure<bool>(AssetErrors.NotFound(request.Id));
    }
}
