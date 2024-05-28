using Domain;

namespace Application;

public interface IDepartmentRepository
{
    Task<DepartmentResponse?> CreateADepartmentAsync(Department department);
    Task<IEnumerable<DepartmentResponse>> GetAllDepartmentsAsync();
    Task<bool> CheckIfIsDepartmentIsUniqueAsync(string name);
    Task<Department?> GetDepartmentByNameAsync(string name);
    Task<bool> CheckIfADepartmentExistsAsync(string name);
    Task<bool> DeleteADepartmentByNameAsync(string name);
}
