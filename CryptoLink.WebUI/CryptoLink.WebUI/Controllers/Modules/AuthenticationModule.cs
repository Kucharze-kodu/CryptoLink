using CryptoLink.Application.Features.Users.Register;
using CryptoLink.Application.Features.Users.RegisterInit;
using MediatR;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using static GameGather.Api.Common.HttpResultsExtensions;

namespace GameGather.Api.Modules;

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

    }
}