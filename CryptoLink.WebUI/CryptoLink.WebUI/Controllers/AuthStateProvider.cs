using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using static CryptoLink.WebUI.Controllers.Common.HttpResultsExtensions;

namespace CryptoLink.WebUI.Controllers.Modules;

public static class AuthStateProvider
{
    public static void AddAuthStateProvider(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/authC/logout", async (HttpContext httpContext) =>
        {
            //await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //httpContext.Response.Headers.Append("Clear-Site-Data", "\"Cookies\"");
            return Results.Ok();
        });


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





