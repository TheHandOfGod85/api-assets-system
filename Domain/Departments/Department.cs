namespace Domain;

public class Department
{
    public string Name { get; private set; } = default!;
    public Guid? AssetId { get; private set; }
    public Asset? Asset { get; private set; }
    public Department(string name)
    {
        Name = name;
    }

    private Department() { }
}
