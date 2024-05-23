using Application;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Web.API;
[ApiController]
public class DepartmentController(IMediator mediator) : ControllerBase
{


    [HttpPost(Endpoints.Departments.CreateADepartment)]
    public async Task<IActionResult> CreateADepartment(
        [FromBody] CreateADepartment request,
        CancellationToken cancellationToken)
    {
        Result<DepartmentResponse> result = await mediator.Send(request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblemDetails();
    }

    [HttpGet(Endpoints.Departments.GetAllDepartments)]
    public async Task<IActionResult> GetAllDepartments(CancellationToken cancellationToken)
    {
        Result<IEnumerable<DepartmentResponse>> result = await mediator.Send(new GetAllDepartments(), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblemDetails();
    }
}
