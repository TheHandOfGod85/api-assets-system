using Application;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Web.API;
[ApiController]
public class DepartmentController(IMediator mediator) : ControllerBase
{


    [HttpPost(Endpoints.Departments.Create)]
    public async Task<IActionResult> Create(
        [FromBody] CreateADepartment request,
        CancellationToken cancellationToken)
    {
        Result<DepartmentResponse> result = await mediator.Send(request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblemDetails();
    }

    [HttpGet(Endpoints.Departments.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        Result<IEnumerable<Department>> result = await _departmentService.GetAllDepartmentsAsync(cancellationToken);
        var departmentRespone = result.Value.MapToDepartmentsResponse();
        return result.IsSuccess ? Ok(departmentRespone) : result.ToProblemDetails();
    }
}
