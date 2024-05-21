using System.ComponentModel.DataAnnotations;

namespace Contracts;

public class CreateDepartmentRequest
{
    [Required(ErrorMessage = "Department name is required")]
    [MaxLength(30, ErrorMessage = "Name must be max 50 characters")]
    public string Name { get; set; } = default!;

}
