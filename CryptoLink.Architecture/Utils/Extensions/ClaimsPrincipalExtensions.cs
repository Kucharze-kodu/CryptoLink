using System.Security.Claims;

namespace CryptoLink.Architecture.Utils.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static int GetUserId(this ClaimsPrincipal? principal)
    {
        var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

        return Convert.ToInt32(userId);
    }

    public static string GetUserName(this ClaimsPrincipal? principal)
    {
        var name = principal.FindFirstValue(ClaimTypes.Name);

        return name;
    }
}