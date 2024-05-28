namespace Domain;

public class Department
{
    public string Name { get; private set; } = default!;
    public Guid? AssetId { get; set; }
    public Asset? Asset { get; set; }
    public Department(string name)
    {
        Name = name;
    }

    private Department() { }
}
