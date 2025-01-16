using Microsoft.AspNetCore.Mvc;
using MyLog.Core.Contracts;
using MyLog.Core.Contracts.Interfaces;
using MyLog.Core.Contracts.Models;

namespace MyLog.Services.Api.Endpoints;

public static class Movements
{
    public static void MapMovementEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/movements/{count}", async ([FromRoute] int count, [FromServices] IMovementsService movementsService, IHttpContextAccessor accessor) =>
        {
            var movements = await movementsService.GetMovementsAsync(count, accessor.HttpContext!.User.Identity!.Name);
            return Results.Ok(movements);
        })
        .WithName("GetMovementList")
        .RequireAuthorization("AdminOrUser");

        routes.MapGet("/movement/{id}", async ([FromRoute] int id, [FromServices] IMovementsService movementsService) =>
        {
            var movement = await movementsService.GetMovementByIdAsync(id);
            return movement is not null ? Results.Ok(movement) : Results.NotFound();
        })
        .WithName("GetMovementDetails")
        .RequireAuthorization("AdminOrUser");

        routes.MapPost("/movement", async ([FromBody] MovementDetailDto movementDetailDto, [FromServices] IMovementsService movementsService) =>
        {
            var result = await movementsService.UpdateMovementAsync(movementDetailDto);
            return result ? Results.Ok() : Results.NotFound();
        })
        .WithName("UpdateMovement")
        .RequireAuthorization("AdminOrUser");

        routes.MapDelete("/movements/{id}", async ([FromRoute] int id, [FromServices] IMovementsService movementsService) =>
        {
            var result = await movementsService.DeleteMovementByIdAsync(id);
            return result ? Results.Ok() : Results.NotFound();
        })
        .WithName("DeleteMovement")
        .RequireAuthorization("AdminOnly");

        routes.MapGet("/movementbyuser/{userName}", async ([FromRoute] string userName, [FromServices] IMovementsService movementsService, [FromServices] IReportService reportService, IHttpContextAccessor accessor) =>
        {
            var movements = await movementsService.GetMovementsByUserAsync(userName);
            await reportService.CreateMovementReportAsync(movements.ToList());
            return Results.Ok(movements);
        })
        .WithName("GetMovementByUser")
        .RequireAuthorization("AdminOrUser");
    }
}
