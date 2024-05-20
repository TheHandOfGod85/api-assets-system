
using SharedKernel;

namespace Domain;

public static class AssetErrors
{
    public static Error NotFound(Guid id) => Error.NotFound(
        "Asset.NotFound", $"The asset with the Id = '{id}' was not found");
    public static readonly Error SerialNumberNotUnique = Error.Conflict(
        "Asset.SerialNumberNotUnique", "The provided serial number is not unique");

}
