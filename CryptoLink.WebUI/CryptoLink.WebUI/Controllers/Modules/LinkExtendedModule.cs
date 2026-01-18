using CryptoLink.Application.Features.LinkExtendeds.Commands.CreateLinkExtendeds;
using CryptoLink.Application.Features.LinkExtendeds.Commands.DeleteLinkExntededs;
using CryptoLink.Application.Features.LinkExtendeds.Commands.EditLinkExtendeds;
using CryptoLink.Application.Features.LinkExtendeds.Queries.GetAllLinkExtended;
using CryptoLink.Application.Features.LinkExtendeds.Queries.LoadLinkExtended;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static CryptoLink.WebUI.Controllers.Common.HttpResultsExtensions;


namespace CryptoLink.WebUI.Controllers.Modules
{
    public static class LinkExtendedModule
    {
        public static void AddLinkExtendedModule(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/linkExtended", async (
                [FromBody] CreateLinkExtendedCommand command,
                [FromServices] ISender sender) =>
            {
                var response = await sender.Send(command);

                return response.Match(
                    result => Ok(result),
                    errors => Problem(errors));
            }).RequireAuthorization();

            app.MapDelete("/api/linkExtended", async (
                [FromBody] DeleteLinkExtendedCommand command,
                [FromServices] ISender sender) =>
            {
                var response = await sender.Send(command);

                return response.Match(
                    result => Ok(result),
                    errors => Problem(errors));
            }).RequireAuthorization();

            app.MapPut("/api/linkExtended", async (
                [FromBody] EditLinkExtendedCommand command,
                [FromServices] ISender sender) =>
            {
                var response = await sender.Send(command);

                return response.Match(
                    result => Ok(result),
                    errors => Problem(errors));
            }).RequireAuthorization();


            app.MapGet("/api/linkExtended", async (
                [FromServices] ISender sender) =>
                        {
                            var response = await sender.Send(new GetAllLinkExtendedQuery());

                            return response.Match(
                                result => Ok(result),
                                errors => Problem(errors));
                        }
            ).RequireAuthorization();


            app.MapGet("/api/LoadlinkExtended", async (
            [FromBody] LoadLinkExtendedQuery querry,
            [FromServices] ISender sender) =>
                    {
                        var response = await sender.Send(querry);

                        return response.Match(
                            result => Ok(result),
                            errors => Problem(errors));
                    }
            );
        }
    }
}
