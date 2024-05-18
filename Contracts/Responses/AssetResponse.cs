namespace Contracts;

public class AssetResponse
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
    public required string SerialNUmber { get; init; }
    public string? Description { get; init; }
    public required string Department { get; init; }
}
