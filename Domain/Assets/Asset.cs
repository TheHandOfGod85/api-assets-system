namespace Domain;

public class Asset
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = default!;
    public string SerialNumber { get; private set; } = default!;
    public Department? Department { get; private set; }
    public string? Description { get; private set; }

    public Asset(
        string name,
        string serialNumber,
        Department? department,
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

    public void UpdateAsset(
        Guid id,
        string name,
        string serialNumber,
        Department? department,
        string? description)
    {
        Id = id;
        Name = name;
        SerialNumber = serialNumber;
        Department = department;
        Description = description;
    }
    public void AddDepartment(Department? department)
    {
        Department = department;
    }
}
