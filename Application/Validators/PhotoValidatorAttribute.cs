using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Application;

public class PhotoValidatorAttribute : ValidationAttribute
{
    private readonly string[] _validExtensions = { ".jpeg", ".jpg", ".png" };
    private readonly long _maxFileSize = 2 * 1024 * 1024;
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value is IFormFile file)
        {
            if (file.Length > _maxFileSize)
            {
                return new ValidationResult($"File size should not exceed {_maxFileSize / (1024 * 1024)} MB.");
            }
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (Array.IndexOf(_validExtensions, extension) < 0)
            {
                return new ValidationResult("Only JPEG and PNG files are allowed.");
            }
            return ValidationResult.Success!;
        }
        return new ValidationResult("Invalid file.");
    }
}
