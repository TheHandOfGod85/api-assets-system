namespace Contracts;

public class CreateAssetRequest
{
    public string Name { get; init; } = default!;
    public string SerialNumber { get; init; } = default!;
    public string? Description { get; init; }
    public string Department { get; init; } = default!;
}
