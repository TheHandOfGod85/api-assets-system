using FluentValidation;

namespace Application;

public class AssetValidator : AbstractValidator<Asset>
{
    private readonly IAssetRepository _assetRepository;
    public AssetValidator(IAssetRepository assetRepository)
    {
        _assetRepository = assetRepository;

        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Department).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Description).MaximumLength(300);
        RuleFor(x => x.SerialNumber)
        .MustAsync(ValidateSerialNumber)
        .WithMessage("This serial number already exists");
    }
    private async Task<bool> ValidateSerialNumber(
        Asset asset,
        string serialNumber,
        CancellationToken cancellationToken = default)
    {
        var existingAsset = await _assetRepository.IsSerialNumberUnique(serialNumber);
        if (existingAsset)
        {
            return true;
        }

        return existingAsset;
    }
}
