using SharedKernel;

namespace Domain;

public static class DepartmentsErrors
{
    public static readonly Error DepartmentNotUnique = Error.Conflict(
        "Department", "The department is not unique"
    );
    public static Error NotFound(string name) => Error.NotFound(
        "Department.NotFound", $"The department with the name = '{name}' was not found, please create first a department before assign it");
    public static Error CannotDelete(string name) => Error.Conflict(
        "Department.CannotDelete", $"The department with the name = '{name}' cannot be deleted as it is assigned to assets, consider changing name instead");
    public static readonly Error DepartmentCreationError = Error.Conflict(
            "Department.NotCreated", "There is a problem creating the department"
        );
}
