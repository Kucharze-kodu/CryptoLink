using CryptoLink.Application.Features.BookWords.Commands.CreateBookWords;
using CryptoLink.Application.Features.BookWords.Commands.DeleteBookWords;
using CryptoLink.Application.Features.BookWords.Commands.RandomLinks;
using CryptoLink.Application.Features.BookWords.Queries.GetAllBookWordCategory;
using CryptoLink.Application.Features.BookWords.Queries.GetAllCategory;
using CryptoLink.Application.Features.LinkExtendeds.Queries.GetAllLinkExtended;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static CryptoLink.WebUI.Controllers.Common.HttpResultsExtensions;

namespace CryptoLink.WebUI.Controllers.Modules
{
    public static class LinkWordModule
    {
        public static void AddLinkWordModule(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/bookword", async (
             [FromBody] CreateBookWordCommand command,
             [FromServices] ISender sender) =>
            {
                var response = await sender.Send(command);

                return response.Match(
                    result => Ok(result),
                    errors => Problem(errors));
            }).RequireAuthorization();


            app.MapDelete("/api/bookword", async (
             [FromBody] DeleteBookWordCommand command,
             [FromServices] ISender sender) =>
            {
                var response = await sender.Send(command);

                return response.Match(
                    result => Ok(result),
                    errors => Problem(errors));
            }).RequireAuthorization();


            app.MapPost("/api/generateLink", async (
             [FromBody] CreateRandomLinkCommand command,
             [FromServices] ISender sender) =>
            {
                var response = await sender.Send(command);

                return response.Match(
                    result => Ok(result),
                    errors => Problem(errors));
            }).RequireAuthorization();

            app.MapGet("/api/Categorybookword", async (
            [FromServices] ISender sender) =>
            {
                var response = await sender.Send(new GetAllCategoryQuery());

                return response.Match(
                    result => Ok(result),
                    errors => Problem(errors));
            }
            ).RequireAuthorization();


            app.MapGet("/api/bookword", async (
            [FromBody] GetAllBookWordCategoryQuery query,
            [FromServices] ISender sender) =>
            {
                var response = await sender.Send(query);

                return response.Match(
                    result => Ok(result),
                    errors => Problem(errors));
            }
            ).RequireAuthorization();

        }
    }
}
