using System.Security.Claims;
using DPManagement.Application.Interfaces;

namespace DPManagement.API.Services;

public class UserContextService : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => GetClaim("sub") ?? GetClaim(ClaimTypes.NameIdentifier) ?? GetClaim("id");
    
    public string? UserName => GetClaim("name") 
                               ?? _httpContextAccessor.HttpContext?.User?.Identity?.Name 
                               ?? GetClaim(ClaimTypes.Name) 
                               ?? GetClaim("unique_name");

    private string? GetClaim(string type) => _httpContextAccessor.HttpContext?.User?.FindFirst(type)?.Value;
}
