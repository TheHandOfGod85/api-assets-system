using Application;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Web.API;
[ApiController]
[Authorize]
public class DepartmentController(IMediator mediator) : ControllerBase
{


    [HttpPost(Endpoints.Departments.CreateADepartment)]
    public async Task<IActionResult> CreateADepartment(
        [FromBody] CreateADepartment request,
        CancellationToken cancellationToken)
    {
        Result<DepartmentResponse?> result = await mediator.Send(request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblemDetails();
    }

    [HttpGet(Endpoints.Departments.GetAllDepartments)]
    public async Task<IActionResult> GetAllDepartments(CancellationToken cancellationToken)
    {
        Result<IEnumerable<DepartmentResponse>> result = await mediator.Send(new GetAllDepartments(), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblemDetails();
    }

    [HttpDelete(Endpoints.Departments.DeleteADepartmentByName)]
    public async Task<IActionResult> DeleteADepartmentByName(
        [FromRoute] string name,
        CancellationToken cancellationToken)
    {
        Result result = await mediator.Send(new DeleteADepartmentByName { Name = name }, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblemDetails();
    }
    [HttpPatch(Endpoints.Departments.ChangeDepartmentName)]
    public async Task<IActionResult> ChangeDepartmentName(
        [FromBody] ChangeDepartmentName request,
        [FromRoute] string name,
        CancellationToken cancellationToken)
    {
        request.Name = name;
        Result<bool> result = await mediator.Send(request, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblemDetails();
    }
}
