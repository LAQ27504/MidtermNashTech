using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;

public class TokenInfoService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TokenInfoService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public (Guid UserId, string UserName, string Role) GetTokenInfo()
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user == null || !user.Identity.IsAuthenticated)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userName = user.FindFirst(JwtRegisteredClaimNames.Name)?.Value;
        var userRole = user.FindFirst(ClaimTypes.Role)?.Value;

        if (
            string.IsNullOrEmpty(userId)
            || string.IsNullOrEmpty(userName)
            || string.IsNullOrEmpty(userRole)
        )
        {
            throw new InvalidOperationException("User ID or Name or Role not found in token.");
        }

        return (Guid.Parse(userId), userName, userRole);
    }
}
