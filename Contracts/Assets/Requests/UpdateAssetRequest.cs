using System.ComponentModel.DataAnnotations;

namespace Contracts;

public class UpdateAssetRequest
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; init; } = default!;
    [Required(ErrorMessage = "Serial number is required")]
    [MaxLength(50, ErrorMessage = "Serial number must be max 50 characters")]
    public string SerialNumber { get; init; } = default!;
    [MaxLength(300, ErrorMessage = "Serial number must be max 300 characters")]
    public string? Description { get; init; }
    [Required(ErrorMessage = "Department is required")]
    [MaxLength(30, ErrorMessage = "Name must be max 50 characters")]
    public string Department { get; init; } = default!;
}
