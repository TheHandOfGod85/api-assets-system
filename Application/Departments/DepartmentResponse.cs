using Domain;

namespace Application;

public class DepartmentResponse
{
    public string Name { get; set; } = default!;
    public List<string>? AssetName { get; set; }

}


public static class DepartmentMappings
{
    public static Department MapToDepartment(this DepartmentResponse department)
    {
        return new Department
        (
            department.Name
        );
    }
}
