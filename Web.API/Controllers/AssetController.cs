using Application;
using Contracts;
using Domain;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Web.API;
[ApiController]
public class AssetController(
    IAssetService assetService) : ControllerBase
{
    private readonly IAssetService _asseService = assetService;

    [HttpPost(Endpoints.Assets.Create)]
    public async Task<IActionResult> Create(
        [FromBody] CreateAssetRequest request,
        CancellationToken cancellationToken)
    {
        var asset = request.MapToAsset();
        Result<bool> result = await _asseService.CreateAsync(asset, cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(Get), new { id = asset.Id }, asset) : result.ToProblemDetails();
    }

    [HttpGet(Endpoints.Assets.Get)]
    public async Task<IActionResult> Get(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        Result<Asset?> result = await _asseService.GetByIdAsync(id, cancellationToken);
        return result.IsSuccess
        ? Ok(result.Value?.MapToAssetResponse())
        : result.ToProblemDetails();
    }

    [HttpGet(Endpoints.Assets.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        Result<IEnumerable<Asset>> result = await _asseService.GetAllAsync(cancellationToken);
        var assetsResponse = result.Value.MapToAssetsResponse();
        return result.IsSuccess ? Ok(assetsResponse) : result.ToProblemDetails();
    }

    [HttpPut(Endpoints.Assets.Update)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateAssetRequest request,
        CancellationToken cancellationToken)
    {
        var asset = request.MapToAsset(id);
        Result<Asset> result = await _asseService.UpdateAsync(asset, cancellationToken);
        return result.IsSuccess
        ? Ok(result.Value.MapToAssetResponse())
        : result.ToProblemDetails();
    }

    [HttpDelete(Endpoints.Assets.Delete)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        Result<bool> result = await _asseService.DeleteByIdAsync(id, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblemDetails();
    }
}
