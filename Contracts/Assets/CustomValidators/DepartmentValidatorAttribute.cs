using System.ComponentModel.DataAnnotations;
using Application;

namespace Contracts;

public class DepartmentValidatorAttribute : ValidationAttribute
{
    private readonly string[] _allowedDepartments = { "fruit", "assembly", "prep" };

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string department && _allowedDepartments.Contains(department.ToLower()))
        {
            return ValidationResult.Success!;
        }
        return new ValidationResult($"The department must be one of the following: {string.Join(", ", _allowedDepartments)}.");
    }
}
