using Domain;
using MediatR;
using SharedKernel;

namespace Application;

public class GetTheAssetById : IRequest<Result<AssetResponse?>>
{
    public Guid Id { get; set; }
}

public class GetTheAssetByIdHandler : IRequestHandler<GetTheAssetById, Result<AssetResponse?>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetTheAssetByIdHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AssetResponse?>> Handle(GetTheAssetById request, CancellationToken cancellationToken)
    {
        var asset = await _unitOfWork.Assets.GetTheAssetByIdAsync(request.Id);
        if (asset is null) return Result.Failure<AssetResponse?>(AssetErrors.NotFound(request.Id));
        return Result.Success<AssetResponse?>(asset);
    }
}
