using System.Security.Claims;
using FuelTracker.API.Models;

namespace FuelTracker.API.Services;

public class UserContextService(IHttpContextAccessor context)
{
    public Guid GetUserId()
    {
        var contextHttpContext = context.HttpContext
                                 ?? throw new InvalidOperationException(
                                     "The HttpContext is null, but it should not be");
        var claim = contextHttpContext.User.FindFirst(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("No NameIdentifier claim is found");
        
        return Guid.Parse(claim.Value);
    }

    public string GetUsername()
    {
        var contextHttpContext = context.HttpContext
                                 ?? throw new InvalidOperationException(
                                     "The HttpContext is null, but it should not be");
        
        var username = contextHttpContext.User.Identity?.Name 
                       ?? contextHttpContext.User.FindFirst("unique_name")?.Value
                       ?? throw new UnauthorizedAccessException("No Name claim is found");
        
        return username;
    }

    public UserResponse GetCurrentUser()
    {
        return new UserResponse(GetUserId(), GetUsername());
    }
}