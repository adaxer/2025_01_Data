﻿using Microsoft.AspNetCore.Mvc;
using MyLog.Core.Logic;

namespace MyLog.Services.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapGet("/movements/{count}", async ([FromRoute] int count, [FromServices] MovementsService movementsService, IHttpContextAccessor accessor) =>
        {
            var movements = await movementsService.GetMovementsAsync(count, accessor.HttpContext!.User.Identity!.Name);
            return movements;
        })
        .WithName("GetMovementList")
        .RequireAuthorization("AdminOrUser");
        return app;
    }
}
