namespace Domain;

public class Department
{
    private readonly List<Asset>? _assets = new();
    public string Name { get; private set; } = default!;
    public IReadOnlyList<Asset>? Assets => _assets?.AsReadOnly();
    public Department(string name, List<Asset>? assets = null)
    {
        Name = name;
        _assets = assets;
    }

    private Department() { }


    public void ChangeName(string name)
    {
        Name = name;
    }
}
