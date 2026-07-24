using System.Security.Claims;

namespace RssReader.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor)
{
    public int UserId =>
        int.Parse(httpContextAccessor.HttpContext!
            .User.FindFirst(ClaimTypes.NameIdentifier)!
            .Value);
}
