namespace Application;

public class Asset
{
    public required Guid Id { get; init; }
    public required string Name { get; set; }
    public required string SerialNumber { get; set; }
    public required string Department { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; }

}
