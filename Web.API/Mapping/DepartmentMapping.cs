using Contracts;
using Domain;

namespace Web.API;

public static class DepartmentMapping
{
    public static Department MapToDepartment(this CreateDepartmentRequest request)
    {
        return new Department
        (
            request.Name
        );
    }
    public static DepartmentResponse MapToDepartmentResponse(this Department department)
    {
        return new DepartmentResponse
        {
            Name = department.Name
        };
    }

    public static DepartmentsResponse MapToDepartmentsResponse(this IEnumerable<Department> departments)
    {
        return new DepartmentsResponse
        {
            Departments = departments.Select(MapToDepartmentResponse)
        };
    }
}
