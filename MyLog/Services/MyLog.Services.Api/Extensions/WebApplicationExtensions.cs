using Microsoft.AspNetCore.Mvc;
using MyLog.Core.Logic;

namespace MyLog.Services.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapGet("/movements/{count}", async ([FromRoute] int count, [FromServices] MovementsService movementsService) =>
        {
            var movements = await movementsService.GetMovementsAsync(count);
            return movements;
        })
        .WithName("GetMovementList");
        return app;
    }
}
