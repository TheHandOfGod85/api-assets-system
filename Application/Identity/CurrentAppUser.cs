using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Application;

public class CurrentAppUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentAppUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public Guid AppUserId => Guid.Parse(
        _httpContextAccessor.HttpContext?.User
        .FindFirstValue("AppUserId") ?? default(Guid).ToString()
    );
}
