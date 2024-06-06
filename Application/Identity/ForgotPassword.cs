using System.ComponentModel.DataAnnotations;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SharedKernel;

namespace Application;

public class ForgotPassword : IRequest<Result<string>>
{
    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; } = default!;
}

public class ForgotPasswordHandler : IRequestHandler<ForgotPassword, Result<string>>
{
    private readonly UserManager<AppUser> _userManager;
    private IdentityService _identityService;

    public ForgotPasswordHandler(
        UserManager<AppUser> userManager,
        IdentityService identityService)
    {
        _userManager = userManager;
        _identityService = identityService;
    }

    public async Task<Result<string>> Handle(ForgotPassword request, CancellationToken cancellationToken)
    {
        var appUser = await _userManager.FindByEmailAsync(request.EmailAddress);
        if (appUser is null) return Result.Failure<string>(AppUserErrors.UserNotFound(request.EmailAddress));
        var token = await _userManager.GeneratePasswordResetTokenAsync(appUser);
        _identityService.SendEmailForgotPassword(appUser, token);
        return Result.Success(token);
    }
}
