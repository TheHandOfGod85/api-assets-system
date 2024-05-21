namespace Contracts;

public class AssetResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
    public string SerialNumber { get; init; } = default!;
    public string? Description { get; init; }
    public DepartmentResponse? Department { get; init; }
}
