using Application.Security;
using System.Security.Claims;

namespace CarHub.Api.Infrastructure;

public class HttpUserContext : IUserContext
{
    private readonly int _userId;

    public HttpUserContext(IHttpContextAccessor httpContextAccessor)
    {
        var id = httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        bool isOkay = Int32.TryParse(id, out var parsedId);

        _userId = isOkay ? parsedId : 0;
    }

    public int UserId => _userId;



    public int MustGetUserId()
    {
        if (_userId == 0)
        {
            throw new BadHttpRequestException("User must login");
        }

        return _userId;
    }
}
