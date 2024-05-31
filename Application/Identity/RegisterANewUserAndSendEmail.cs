using System.ComponentModel.DataAnnotations;
using MediatR;
using SharedKernel;
using Microsoft.AspNetCore.Identity;
using Domain;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;


namespace Application;

public class RegisterANewUserAndSendEmail : IRequest<Result>
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
    [EnumDataType(typeof(Role), ErrorMessage = "Invalid role specified. (Admin or AppUser)")]
    public Role Role { get; set; } = Role.AppUser;

}

public class RegisterANewUserAndSendEmailHandler : IRequestHandler<RegisterANewUserAndSendEmail, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IdentityService _identityService;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IEmailSender _emailSender;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public RegisterANewUserAndSendEmailHandler(
        IUnitOfWork unitOfWork,
        UserManager<IdentityUser> userManager,
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



    public async Task<Result> Handle(RegisterANewUserAndSendEmail request, CancellationToken cancellationToken)
    {
        try
        {
            await _unitOfWork.StartTransactionAsync(cancellationToken);
            var identity = await CreateIdentityUserAsync(request);
            if (identity == null) return Result.Failure(AppUserErrors.UserRegistrationFailed);
            var appUser = await CreateAppUserAsync(request, identity.Value);
            if (appUser == null) return Result.Failure(AppUserErrors.UserAlreadyExists);
            var roleClaim = await AssignRole(request.Role, identity.Value);
            var additionalsClaims = GetIdentityAndAppUserClaims(identity.Value, appUser);
            additionalsClaims.Add(roleClaim);
            await _userManager.AddClaimsAsync(identity.Value, additionalsClaims);
            await _unitOfWork.SubmitTransactionAsync(cancellationToken);
            var token = GetJwtString(identity.Value, additionalsClaims);
            SendEmail(appUser, token);
            return Result.Success();
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
        var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;
        var verifyUrl = $"{origin}/authentication/completeRegistration/?token={token}";
        var htmlContent =
        $"<strong>{verifyUrl}</strong><br/><a href=\"#\">Click here to complete the registration</a><br/><strong>Or click below to resend the verification link</strong><br/><a href=\"#\">resend</a>";

        string receiverEmail = appUser.EmailAddress;
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

    private async Task<Claim> AssignRole(Role requestRole, IdentityUser identity)
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
            await _userManager.AddToRoleAsync(identity, "Admin");
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
            await _userManager.AddToRoleAsync(identity, "AppUser");
            roleClaim = new Claim(ClaimTypes.Role, "AppUser");
            return roleClaim;
        }
    }

    private async Task<AppUser?> CreateAppUserAsync(RegisterANewUserAndSendEmail request, IdentityUser identity)
    {

        var appUserId = await _unitOfWork.AppUsers.RegisterAppUserAsync
            (
                identity.Id,
                request.FirstName,
                request.LastName,
                request.EmailAddress
            );
        if (appUserId == null) return null;
        var appUser = new AppUser
        (
            identity.Id,
            request.FirstName,
            request.LastName,
            request.EmailAddress,
            appUserId
        );
        return appUser;
    }

    private async Task<Result<IdentityUser?>?> CreateIdentityUserAsync(RegisterANewUserAndSendEmail request)
    {
        var identity = new IdentityUser { Email = request.EmailAddress, UserName = request.EmailAddress };
        var createdIdentity = await _userManager.CreateAsync(identity);
        if (!createdIdentity.Succeeded) return null;
        return identity;
    }

    private List<Claim> GetIdentityAndAppUserClaims(IdentityUser identity, AppUser appUser)
    {
        return new List<Claim>
        {
            new("IdentityId", identity.Id),
            new("AppUserId", appUser.Id.ToString()),
            new("EmailAddress", identity.Email!),
            new("FirstName", appUser.FirstName),
            new("LastName", appUser.LastName)
        };
    }

    private string GetJwtString(IdentityUser identity, IEnumerable<Claim> additionalClaims)
    {
        var claimsIdentity = new ClaimsIdentity(new Claim[]
        {
            new(JwtRegisteredClaimNames.Sub, identity.Email ?? throw new InvalidOperationException()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Email, identity.Email),
        });
        claimsIdentity.AddClaims(additionalClaims);

        var token = _identityService.CreateSecurityToken(claimsIdentity);
        return _identityService.WriteToken(token);
    }
}
public enum Role
{
    Admin,
    AppUser
}





