namespace Application;

public class AssetResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string SerialNumber { get; set; } = default!;
    public string? DepartmentName { get; set; }
    public string? Description { get; set; }
}

