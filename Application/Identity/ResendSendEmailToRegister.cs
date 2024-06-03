using System.ComponentModel.DataAnnotations;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SharedKernel;

namespace Application;

public class ResendEmailToRegister : IRequest<Result<string>>
{
    [Required]
    public string EmailAddress { get; set; } = default!;
}

public class ResendEmailToRegisterHandler : IRequestHandler<ResendEmailToRegister, Result<string>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IdentityService _identityService;
    public ResendEmailToRegisterHandler(
        UserManager<AppUser> userManager,
        IdentityService identityService)
    {
        _userManager = userManager;
        _identityService = identityService;
    }


    public async Task<Result<string>> Handle(ResendEmailToRegister request, CancellationToken cancellationToken)
    {
        var appUser = await _userManager.FindByEmailAsync(request.EmailAddress);
        if (appUser == null) return Result.Failure<string>(AppUserErrors.UserNotFound(request.EmailAddress));
        var claims = await _identityService.ComputeClaims(appUser);
        var token = _identityService.GetJwtString(appUser, claims);
        _identityService.SendEmailRegistration(appUser, token);
        return Result.Success(token);
    }
}