using System.ComponentModel.DataAnnotations;
namespace Application;

public class NoWhiteSpaceAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is string str && str.Contains(' '))
        {
            return new ValidationResult($"The {value.ToString()} cannot contain white spaces.");
        }

        return ValidationResult.Success;
    }
}

