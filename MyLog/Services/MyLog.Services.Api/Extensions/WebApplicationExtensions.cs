using MyLog.Services.Api.Endpoints;

namespace MyLog.Services.Api.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapMovementEndpoints();
      
        return app;
    }
}
