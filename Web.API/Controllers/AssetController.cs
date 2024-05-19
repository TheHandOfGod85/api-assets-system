using Application;
using Contracts;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Web.API;
[ApiController]
public class AssetController(
    IAssetService assetService) : ControllerBase
{
    private readonly IAssetService _asseService = assetService;

    [HttpPost(Endpoints.Assets.Create)]
    public async Task<IActionResult> Create([FromBody] CreateAssetRequest request)
    {
        var asset = request.MapToAsset();
        Result<bool> result = await _asseService.CreateAsync(asset);
        return result.IsSuccess ? CreatedAtAction(nameof(Get), new { id = asset.Id }, asset) : result.ToProblemDetails();
    }

    [HttpGet(Endpoints.Assets.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        Result<Asset?> result = await _asseService.GetByIdAsync(id);
        var response = result.Value.MapToAssetResponse();
        return result.IsSuccess ? Ok(response) : result.ToProblemDetails();
    }

    [HttpGet(Endpoints.Assets.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        Result<IEnumerable<Asset>> result = await _asseService.GetAllAsync();
        var assetsResponse = result.Value.MapToAssetsResponse();
        return result.IsSuccess ? Ok(assetsResponse) : result.ToProblemDetails();
    }

    [HttpPut(Endpoints.Assets.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateAssetRequest request)
    {
        var asset = request.MapToAsset(id);
        Result<bool> result = await _asseService.UpdateAsync(asset);

        // var response = updatedAsset.MapToAssetResponse();

        return result.IsSuccess ? NoContent() : result.ToProblemDetails();
    }

    [HttpDelete(Endpoints.Assets.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        Result<bool> result = await _asseService.DeleteByIdAsync(id);

        return result.IsSuccess ? NoContent() : result.ToProblemDetails();
    }
}
