namespace Domain;

public class Asset
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = default!;
    public string SerialNumber { get; private set; } = default!;
    public string Department { get; private set; } = default!;
    public string? Description { get; private set; }

    public Asset(
        string name,
        string department,
        string serialNumber,
        string? description,
        Guid? id = null
        )
    {
        Id = id ?? Guid.NewGuid();
        Name = name;
        Department = department;
        Description = description;
        SerialNumber = serialNumber;
    }
    private Asset() { }

    public Asset UpdateAsset(
        Guid id,
        string name,
        string department,
        string serialNumber,
        string? description)
    {
        return new Asset
        (
            name,
            department,
            serialNumber,
            description,
            id
        );
    }
}
