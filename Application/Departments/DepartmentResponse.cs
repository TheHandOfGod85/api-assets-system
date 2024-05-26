using Domain;

namespace Application;

public class DepartmentResponse
{
    public string Name { get; set; } = default!;

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
