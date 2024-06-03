using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using SharedKernel;

namespace Application;

public class CompleteRegistrationFromEmail : IRequest<Result<RegistrationResult>>
{
    [Required]
    public string Token { get; set; } = default!;
    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; } = default!;
    [Required]
    public string Password { get; set; } = default!;
}

public class CompleteRegistrationFromEmailHandler : IRequestHandler<CompleteRegistrationFromEmail, Result<RegistrationResult>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;
    private readonly IdentityService _identityService;


    public CompleteRegistrationFromEmailHandler(
        IUnitOfWork unitOfWork,
        UserManager<AppUser> userManager,
        IdentityService identityService)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _identityService = identityService;
    }

    public async Task<Result<RegistrationResult>> Handle(CompleteRegistrationFromEmail request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.StartTransactionAsync(cancellationToken);
            var isTokenValid = _identityService.VerifyToken(request.Token);
            if (!isTokenValid) return Result.Failure<RegistrationResult>(AppUserErrors.InvalidToken);
            var appUser = await _userManager.FindByEmailAsync(request.EmailAddress);
            if (appUser == null) return Result.Failure<RegistrationResult>(AppUserErrors.UserNotFound(request.EmailAddress));
            if (appUser.EmailConfirmed) return Result.Failure<RegistrationResult>(AppUserErrors.UserAlreadyConfirmed);
            appUser.EmailConfirmed = true;
            await _userManager.AddPasswordAsync(appUser, request.Password);
            await _userManager.UpdateAsync(appUser);
            var additionalsClaims = await _identityService.ComputeClaims(appUser);
            var token = _identityService.GetJwtString(appUser, additionalsClaims);
            var registrationResult = new RegistrationResult(
                appUser.FirstName,
                appUser.LastName,
                appUser.Email!,
                token);
            await _unitOfWork.SubmitTransactionAsync(cancellationToken);
            return Result.Success(registrationResult);

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RevertTransactionAsync(cancellationToken);
            throw;
        }
    }
}

public record RegistrationResult(
    string firstaName,
    string lastName,
    string emailAddress,
    string token);
