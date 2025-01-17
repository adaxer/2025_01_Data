namespace MyLog.Client.Razor;

public static class BuilderExtensions
{

    public static WebApplicationBuilder AddAuthenticatingHttpClient(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient("Api", (sp, client) =>
        {
            var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
            var accessToken = httpContextAccessor.HttpContext?.User?.FindFirst("AccessToken")?.Value;

            if (!string.IsNullOrEmpty(accessToken))
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }
            client.BaseAddress = new Uri(builder.Configuration["Api:Url"] ?? throw new ArgumentNullException("Please add Api-Url to configuration"));
        });
        return builder;
    }
}
