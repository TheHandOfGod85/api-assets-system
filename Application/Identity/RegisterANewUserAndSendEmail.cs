using System.ComponentModel.DataAnnotations;
using MediatR;
using SharedKernel;
using Microsoft.AspNetCore.Identity;
using Domain;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;


namespace Application;

public class RegisterANewUserAndSendEmail : IRequest<Result<string>>
{
    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; } = default!;
    [Required]
    [MaxLength(50, ErrorMessage = "Max 50 characters")]
    public string FirstName { get; set; } = default!;
    [Required]
    [MaxLength(50, ErrorMessage = "Max 50 characters")]
    public string LastName { get; set; } = default!;
    // [ValidUserRole]
    [EnumDataType(typeof(Role), ErrorMessage = "Invalid role specified. (Admin or AppUser)")]
    public Role Role { get; set; } = default!;

}

public class RegisterANewUserAndSendEmailHandler : IRequestHandler<RegisterANewUserAndSendEmail, Result<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;
    private readonly IdentityService _identityService;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IEmailSender _emailSender;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public RegisterANewUserAndSendEmailHandler(
        IUnitOfWork unitOfWork,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IdentityService identityService,
        IEmailSender emailSender,
        IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _identityService = identityService;
        _roleManager = roleManager;
        _emailSender = emailSender;
        _httpContextAccessor = httpContextAccessor;
    }



    public async Task<Result<string>> Handle(RegisterANewUserAndSendEmail request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.StartTransactionAsync(cancellationToken);
            var appUser = await CreateAppUserAsync(request);
            if (appUser == null) return Result.Failure<string>(AppUserErrors.UserRegistrationFailed);
            var roleClaim = await AssignRole(request.Role, appUser.Value);
            var additionalsClaims = _identityService.GetAppUserClaims(appUser.Value);
            additionalsClaims.Add(roleClaim);
            await _userManager.AddClaimsAsync(appUser.Value, additionalsClaims);
            var token = _identityService.GetJwtString(appUser.Value, additionalsClaims);
            SendEmail(appUser.Value, token);
            await _unitOfWork.SubmitTransactionAsync(cancellationToken);
            return Result.Success(token);
        }
        catch (AppUserAlreadyExistsException)
        {
            return Result.Failure<string>(AppUserErrors.UserAlreadyExists);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await _unitOfWork.RevertTransactionAsync(cancellationToken);
            throw;
        }

    }

    private void SendEmail(AppUser appUser, string token)
    {
        var origin = _httpContextAccessor.HttpContext?.Request.Headers["Origin"].ToString();
        var verifyUrl = $"{origin}/authentication/completeRegistration/?token={token}";
        var htmlContent =
        $"<strong>{verifyUrl}</strong><br/><a href=\"#\">Click here to complete the registration</a><br/><strong>Or click below to resend the verification link</strong><br/><a href=\"#\">resend</a>";

        string receiverEmail = appUser.Email!;
        string receiverName = appUser.LastName + " " + appUser.FirstName;
        string subject = "Registration to AssetsSystem";
        string message = "Please, complete the registration following this link below";

        _emailSender.SendEmail(
            receiverEmail,
            receiverName,
            subject,
            message,
            htmlContent
            );
    }

    private async Task<Claim> AssignRole(Role requestRole, AppUser appUser)
    {
        Claim roleClaim;
        if (requestRole == Role.Admin)
        {
            var role = await _roleManager.FindByNameAsync("Admin");
            if (role == null)
            {
                role = new IdentityRole("Admin");
                await _roleManager.CreateAsync(role);
            }
            await _userManager.AddToRoleAsync(appUser, "Admin");
            roleClaim = new Claim(ClaimTypes.Role, "Admin");
            return roleClaim;
        }
        else
        {
            var role = await _roleManager.FindByNameAsync("AppUser");
            if (role == null)
            {
                role = new IdentityRole("AppUser");
                await _roleManager.CreateAsync(role);
            }
            await _userManager.AddToRoleAsync(appUser, "AppUser");
            roleClaim = new Claim(ClaimTypes.Role, "AppUser");
            return roleClaim;
        }
    }

    private async Task<Result<AppUser?>?> CreateAppUserAsync(RegisterANewUserAndSendEmail request)
    {
        var exist = await _userManager.FindByEmailAsync(request.EmailAddress);
        if (exist != null) throw new AppUserAlreadyExistsException();
        var appUser = new AppUser(request.FirstName, request.LastName, request.EmailAddress, request.EmailAddress);
        var createdIdentity = await _userManager.CreateAsync(appUser);
        if (!createdIdentity.Succeeded) return null;
        return appUser;
    }
}
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Role
{
    Admin,
    AppUser
}





