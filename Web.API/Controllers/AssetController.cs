using Application;
using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Web.API;
[ApiController]
public class AssetController(IAssetService assetService) : ControllerBase
{
    private readonly IAssetService _asseService = assetService;

    [HttpPost(Endpoints.Assets.Create)]
    public async Task<IActionResult> Create([FromBody] CreateAssetRequest request)
    {
        var asset = request.MapToAsset();
        await _asseService.CreateAsync(asset);
        return CreatedAtAction(nameof(Get), new { id = asset.Id }, asset);
    }

    [HttpGet(Endpoints.Assets.Get)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var asset = await _asseService.GetByIdAsync(id);
        if (asset is null)
        {
            return NotFound();
        }
        var response = asset.MapToAssetResponse();
        return Ok(response);
    }

    [HttpGet(Endpoints.Assets.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        var assets = await _asseService.GetAllAsync();
        var assetsResponse = assets.MapToAssetsResponse();
        return Ok(assetsResponse);
    }

    [HttpPut(Endpoints.Assets.Update)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateAssetRequest request)
    {
        var asset = request.MapToAsset(id);
        var updatedAsset = await _asseService.UpdateAsync(asset);
        if (updatedAsset is null)
        {
            return NotFound();
        }
        var response = updatedAsset.MapToAssetResponse();
        return Ok(response);
    }

    [HttpDelete(Endpoints.Assets.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var deleted = await _asseService.DeleteByIdAsync(id);
        if (!deleted)
        {
            return NotFound();
        }
        return Ok();
    }
}
