using MediatR;
using SharedKernel;

namespace Application;

public class GetAllTheAssets : IRequest<Result<IEnumerable<AssetResponse>>>
{

}

public class GetAllTheAssetsHandler : IRequestHandler<GetAllTheAssets, Result<IEnumerable<AssetResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllTheAssetsHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<AssetResponse>>> Handle(GetAllTheAssets request, CancellationToken cancellationToken)
    {
        var assets = await _unitOfWork.Assets.GetAllTheAssetsAsync();
        return Result.Success(assets);
    }
}
