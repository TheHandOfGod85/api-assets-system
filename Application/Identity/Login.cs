using System.ComponentModel.DataAnnotations;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SharedKernel;

namespace Application;

public class Login : IRequest<Result<string>>
{
    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; } = default!;
    [Required]
    public string Password { get; set; } = default!;
}

public class LoginHandler : IRequestHandler<Login, Result<string>>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IdentityService _identityService;

    public LoginHandler(
        IdentityService identityService,
        SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager)
    {
        _identityService = identityService;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<Result<string>> Handle(Login request, CancellationToken cancellationToken)
    {
        var appUser = await _userManager.FindByEmailAsync(request.EmailAddress);
        if (appUser is null) return Result.Failure<string>(AppUserErrors.UserNotFound(request.EmailAddress));
        if (!appUser.EmailConfirmed) return Result.Failure<string>(AppUserErrors.EmailNotConfirmed(request.EmailAddress));
        var result = await _signInManager.CheckPasswordSignInAsync(appUser, request.Password, false);
        if (!result.Succeeded) return Result.Failure<string>(AppUserErrors.InvalidCredentials);
        var claims = await _identityService.ComputeClaims(appUser);
        var token = _identityService.GetJwtString(appUser, claims);
        return Result.Success(token);
    }
}
