namespace Contracts;

public class CreateAssetRequest
{
    public required string Name { get; init; }
    public required string SerialNumber { get; init; }
    public string? Description { get; init; }
    public required string Department { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}
