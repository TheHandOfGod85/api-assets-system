using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application;

public class IdentityService
{
    private readonly JwtSettings? _settings;
    private readonly UserManager<AppUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IEmailSender _emailSender;

    private readonly byte[] _key;

    public IdentityService(
        IOptions<JwtSettings> jwtOptions,
        IHttpContextAccessor httpContextAccessor,
        IEmailSender emailSender,
        UserManager<AppUser> userManager)
    {
        _settings = jwtOptions.Value;
        ArgumentNullException.ThrowIfNull(_settings);
        ArgumentNullException.ThrowIfNull(_settings.SigningKey);
        ArgumentNullException.ThrowIfNull(_settings.Audiences);
        ArgumentNullException.ThrowIfNull(_settings.Audiences[0]);
        ArgumentNullException.ThrowIfNull(_settings.Issuer);
        _key = Encoding.ASCII.GetBytes(_settings?.SigningKey!);
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _emailSender = emailSender;

    }

    private static JwtSecurityTokenHandler TokenHandler => new();

    private SecurityTokenDescriptor GetTokenDescriptor(ClaimsIdentity identity)
    {
        return new SecurityTokenDescriptor()
        {
            Subject = identity,
            Expires = DateTime.Now.AddHours(2),
            Audience = _settings!.Audiences?[0]!,
            Issuer = _settings.Issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key),
                SecurityAlgorithms.HmacSha256Signature)
        };
    }

    public SecurityToken CreateSecurityToken(ClaimsIdentity identity)
    {
        var tokenDescriptor = GetTokenDescriptor(identity);

        return TokenHandler.CreateToken(tokenDescriptor);
    }

    public string WriteToken(SecurityToken token)
    {
        return TokenHandler.WriteToken(token);
    }

    public bool VerifyToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings?.SigningKey!)),
            ValidateIssuer = true,
            ValidIssuer = _settings?.Issuer,
            ValidAudiences = _settings?.Audiences,
            ValidateAudience = true,
            RequireExpirationTime = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);
            return true;
        }
        catch (Exception)
        {
            // Token validation failed
            return false;
        }
    }

    public List<Claim> GetAppUserClaims(AppUser appUser)
    {
        return new List<Claim>
        {
            new("AppUserId", appUser.Id),
            new("EmailAddress", appUser.Email!),
            new("FirstName", appUser.FirstName),
            new("LastName", appUser.LastName)
        };
    }

    public string GetJwtString(AppUser appUser, IEnumerable<Claim> additionalClaims)
    {
        var claimsIdentity = new ClaimsIdentity(
        [
            new(JwtRegisteredClaimNames.Sub, appUser.Email ?? throw new InvalidOperationException()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Email, appUser.Email),
        ]);
        claimsIdentity.AddClaims(additionalClaims);

        var token = CreateSecurityToken(claimsIdentity);
        return WriteToken(token);
    }
    public async Task<List<Claim>> ComputeClaims(AppUser appUser)
    {
        var roles = await _userManager.GetRolesAsync(appUser);
        var role = roles.FirstOrDefault();
        var roleClaim = new Claim(ClaimTypes.Role, role!);
        var additionalsClaims = GetAppUserClaims(appUser);
        additionalsClaims.Add(roleClaim);
        return additionalsClaims;
    }
    public void SendEmailRegistration(AppUser appUser, string token)
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
}
