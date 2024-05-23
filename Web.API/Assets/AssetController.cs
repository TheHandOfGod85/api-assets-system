using Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Web.API;
[ApiController]
public class AssetController(
    IMediator mediator
   ) : ControllerBase
{

    [HttpPost(Endpoints.Assets.CreateAnAsset)]
    public async Task<IActionResult> CreateAnAsset(
        [FromBody] CreateAnAsset request,
        CancellationToken cancellationToken)
    {
        Result<AssetResponse> result = await mediator.Send(request, cancellationToken);
        // return result.IsSuccess ? CreatedAtAction(nameof(GetTheAssetById), new { id = result.Value.Id }, result.Value) : result.ToProblemDetails();
        return result.IsSuccess ? Ok(result.Value) : result.ToProblemDetails();
    }

    [HttpGet(Endpoints.Assets.GetTheAssetById)]
    public async Task<IActionResult> GetTheAssetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        Result<AssetResponse?> result = await mediator.Send(new GetTheAssetById { Id = id }, cancellationToken);
        return result.IsSuccess
        ? Ok(result.Value)
        : result.ToProblemDetails();
    }

    [HttpGet(Endpoints.Assets.GetAllTheAssets)]
    public async Task<IActionResult> GetAllTheAssets(CancellationToken cancellationToken)
    {
        Result<IEnumerable<AssetResponse>> result = await mediator.Send(new GetAllTheAssets(), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblemDetails();
    }

    [HttpPatch(Endpoints.Assets.UpdateTheAssetBasicInfoById)]
    public async Task<IActionResult> UpdateAnAssetById(
        [FromRoute] Guid id,
        [FromBody] UpdateTheAssetBasicInfo request,
        CancellationToken cancellationToken)
    {
        request.Id = id;
        Result<AssetBasicInfo?> result = await mediator.Send(request, cancellationToken);
        return result.IsSuccess
        ? Ok(result.Value)
        : result.ToProblemDetails();
    }

    [HttpDelete(Endpoints.Assets.DeleteAnAsset)]
    public async Task<IActionResult> DeleteAnAsset(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        Result<bool> result = await mediator.Send(new DeleteAnAsset { Id = id }, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblemDetails();
    }
}
