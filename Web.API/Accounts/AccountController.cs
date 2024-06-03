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
        Result<string> result = await mediator.Send(request, cancellationToken);
        return result.IsSuccess
        ? Ok(result.Value)
        : result.ToProblemDetails();
    }
    [HttpPost(Endpoints.Accounts.CompleteRegistrationFromEmail)]
    public async Task<IActionResult> CompleteRegistrationFromEmail(
        [FromBody] CompleteRegistrationFromEmail request,
        CancellationToken cancelToken
    )
    {
        Result<RegistrationResult> result = await mediator.Send(request, cancelToken);
        return result.IsSuccess
        ? Ok(result.Value)
        : result.ToProblemDetails();
    }
    [HttpPost(Endpoints.Accounts.ResendSendEmailToRegister)]
    public async Task<IActionResult> ResendEmailToRegister(
        [FromBody] ResendEmailToRegister request,
        CancellationToken cancellationToken)
    {
        Result<string> result = await mediator.Send(request, cancellationToken);
        return result.IsSuccess
        ? Ok(result.Value)
        : result.ToProblemDetails();
    }
}
