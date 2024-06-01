using Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace Web.API;
[ApiController]
public class AccountController(IMediator mediator) : ControllerBase
{
    [HttpPost(Endpoints.Accounts.RegisterANewUserAndSendEmail)]
    public async Task<IActionResult> RegisterANewUserAndSendEmail(
        [FromBody] RegisterANewUserAndSendEmail request,
        CancellationToken cancellationToken
    )
    {
        Result result = await mediator.Send(request, cancellationToken);
        return result.IsSuccess
        ? Ok()
        : result.ToProblemDetails();
    }
    [HttpPost(Endpoints.Accounts.CompleteRegistrationFromEmail)]
    public async Task<IActionResult> CompleteRegistrationFromEmail(
        [FromBody] CompleteRegistrationFromEmail request,
        CancellationToken cancelToken
    )
    {
        Result result = await mediator.Send(request, cancelToken);
        return result.IsSuccess
        ? Ok()
        : result.ToProblemDetails();
    }
}
