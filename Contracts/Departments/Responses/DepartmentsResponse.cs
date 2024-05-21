namespace Contracts;

public class DepartmentsResponse
{
    public IEnumerable<DepartmentResponse> Departments { get; set; } = Enumerable.Empty<DepartmentResponse>();
}
