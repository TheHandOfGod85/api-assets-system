using System.ComponentModel.DataAnnotations;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SharedKernel;

namespace Application;

public class ChangePassword : IRequest<Result>
{
    [Required]
    public string Token { get; set; } = default!;
    [Required]
    public string NewPassword { get; set; } = default!;
    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; } = default!;
}

public class ChangePasswordHandler : IRequestHandler<ChangePassword, Result>
{
    private readonly UserManager<AppUser> _userManager;

    public ChangePasswordHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result> Handle(ChangePassword request, CancellationToken cancellationToken)
    {
        var appUser = await _userManager.FindByEmailAsync(request.EmailAddress);
        if (appUser is null) return Result.Failure(AppUserErrors.UserNotFound(request.EmailAddress));
        var isSamePassword = await _userManager.CheckPasswordAsync(appUser, request.NewPassword);
        if (isSamePassword) return Result.Failure(AppUserErrors.NewPasswordIsNotSameAsOld);
        var resetPassResult = await _userManager.ResetPasswordAsync(appUser, request.Token, request.NewPassword);
        if (!resetPassResult.Succeeded) return Result.Failure(AppUserErrors.InvalidToken);
        return Result.Success();
    }
}
