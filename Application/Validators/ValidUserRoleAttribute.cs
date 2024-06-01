using System.ComponentModel.DataAnnotations;

namespace Application;

public class ValidUserRoleAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || !Enum.IsDefined(typeof(Role), value))
        {
            return new ValidationResult($"Invalid user role: {value}. Allowed values are Admin and AppUser.");
        }

        return ValidationResult.Success;
    }

}
