using Application;
using Contracts;
using Domain;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Web.API;
[ApiController]
public class DepartmentController(IDepartmentService departmentService) : ControllerBase
{
    private readonly IDepartmentService _departmentService = departmentService;


    [HttpPost(Endpoints.Departments.Create)]
    public async Task<IActionResult> Create(
        [FromBody] CreateDepartmentRequest request,
        CancellationToken cancellationToken)
    {
        var department = request.MapToDepartment();
        Result<bool> result = await _departmentService.CreateAsync(department, cancellationToken);
        return result.IsSuccess ? Created() : result.ToProblemDetails();
    }

    [HttpGet(Endpoints.Departments.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        Result<IEnumerable<Department>> result = await _departmentService.GetAllAsync(cancellationToken);
        var departmentRespone = result.Value.MapToDepartmentsResponse();
        return result.IsSuccess ? Ok(departmentRespone) : result.ToProblemDetails();
    }
}
