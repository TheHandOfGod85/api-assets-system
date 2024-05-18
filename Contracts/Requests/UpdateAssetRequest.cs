namespace Contracts;

public class UpdateAssetRequest
{
    public required string Name { get; init; }
    public required string SerialNumber { get; init; }
    public string? Description { get; init; }
    public required string Department { get; init; }
}
