
namespace CryptoLink.WebUI.Controllers.Modules;

public static class AuthStateProvider
{
    public static void AddAuthStateProvider(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/authC/logout", (HttpContext httpContext) =>
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/",
                Expires = DateTime.UtcNow.AddDays(-1)
            };

            httpContext.Response.Cookies.Append("CookiesAuth", "", cookieOptions);
            httpContext.Response.Headers.Append("Clear-Site-Data", "\"cookies\"");

            return Results.Ok();
        })
            .AllowAnonymous();


        app.MapGet("/api/authC/user-info", (HttpContext httpContext) =>
        {
            var user = httpContext.User;
            if (user?.Identity != null && user.Identity.IsAuthenticated)
            {
                var userName = user.Identity.Name ?? "Unknown";
                var role = user.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value ?? "User";
                return Results.Ok(new
                {
                    IsAuthenticated = true,
                    Role = role,
                    UserName = userName
                });
            }
            else
            {
                return Results.Ok(new
                {
                    IsAuthenticated = false,
                    Role = string.Empty,
                    UserName = string.Empty
                });
            }
        });

    } 
}





