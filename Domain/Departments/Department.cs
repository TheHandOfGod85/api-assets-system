namespace Domain;

public class Department
{
    public string Name { get; private set; } = default!;
    public Department(string name)
    {
        Name = name;
    }

    private Department() { }
}
