using CryptoLink.Application.Features.Users.Login;
using CryptoLink.Application.Features.Users.LoginInit;
using CryptoLink.Application.Features.Users.Register;
using CryptoLink.Application.Features.Users.RegisterInit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static CryptoLink.WebUI.Controllers.Common.HttpResultsExtensions;

namespace CryptoLink.WebUI.Controllers.Modules;

public static class AuthenticationModule
{
    public static void AddAuthenticationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/register/init", async (
            [FromBody] RegisterInitCommand command,
            [FromServices] ISender sender) =>
        {
            var response = await sender.Send(command);

            return response.Match(
                result => Ok(result),
                errors => Problem(errors));
        });

        app.MapPost("/api/auth/register/complete", async (
            [FromBody] RegisterCommand command,
            [FromServices] ISender sender) =>
        {
            var response = await sender.Send(command);

            return response.Match(
                result => Ok(result),
                errors => Problem(errors));
        });




        app.MapPost("/api/auth/login/init", async (
            [FromBody] LoginInitCommand command,
            [FromServices] ISender sender) =>
        {
            var response = await sender.Send(command);

            return response.Match(
                result => Ok(result),
                errors => Problem(errors));
        });

        app.MapPost("/api/auth/login/complete", async (
            [FromBody] LoginCommand command,
            [FromServices] ISender sender,
            HttpContext httpContext) =>
        {
            var response = await sender.Send(command);

            return response.Match(
                result =>
                {
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,    
                        Secure = true,      
                        SameSite = SameSiteMode.Strict, 
                        Expires = result.TokenExpiresOnUtc
                    };

                    httpContext.Response.Cookies.Append("CookiesAuth", result.Token, cookieOptions);
                    return Results.Ok();
                },
                errors => Problem(errors));
        });

        app.MapPost("/api/auth/logout", (HttpContext httpContext) =>
        {
            httpContext.Response.Cookies.Delete("CookiesAuth");
            return Results.Ok();
        });
    }
}
